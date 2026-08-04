# Modular Monolith — Step 1: The Cart Module

This document explains the first step of refactoring the GymStore backend from a single
layered project into a **modular monolith**, and how the extracted **Cart module** is
structured. It is meant to be the reference/template for the next modules (Catalog,
Ordering, Payments).

---

## 1. Why

The backend started as one project (`backend/backend`) where every feature shared one
`GymStoreContext`, one set of `Controllers/Services/Repository` folders, and reached into
each other's data freely. The clearest symptom:

- `OrderRepository.PlaceOrderAsync` read `_context.Carts` and deleted `_context.CartItems`
  inside its own transaction — Ordering was coupled directly to Cart's tables.
- The cart read path joined `Product`/`ProductImages` through shared EF navigations — Cart
  was coupled directly to Catalog's tables.

A modular monolith keeps a single deployable app but splits it into modules with **explicit,
compile-time-enforced boundaries**, so features can evolve (and later be split out)
independently. Cart was chosen as the first module to extract.

**Goal:** preserve all existing HTTP behavior and JSON shapes, while establishing the module
template — CQRS request handling, a public contract, module-owned data, and DI-based
composition.

---

## 2. Solution layout

New projects live under `backend/` alongside the existing host (the host was not moved):

```
backend/
  backend.sln
  backend/                              HOST / composition root (existing "backend" project)
  BuildingBlocks/
    GymStore.BuildingBlocks             CQRS abstractions + dispatcher (no web deps)
  Modules/Cart/
    GymStore.Modules.Cart.Contracts     public cross-module API + DTOs (dependency-free)
    GymStore.Modules.Cart               the module (Domain / Application / Infrastructure / Api)
    GymStore.Modules.Cart.Tests         xUnit tests (EF InMemory)
```

**Reference rules — this graph *is* the boundary:**

| Project | May reference |
|---|---|
| `GymStore.Modules.Cart` | BuildingBlocks, Cart.Contracts, EF Core, caching abstractions |
| `backend` (host) | BuildingBlocks, Cart.Contracts, Cart module |
| Ordering code (in host) | **Cart.Contracts only** — never Cart internals |
| `GymStore.Modules.Cart.Contracts` | nothing |

Because Ordering can only see `Cart.Contracts`, it is *physically unable* to reach into
Cart's tables or internals — the compiler enforces "minimal coupling," not a convention doc.

---

## 3. CQRS building blocks

`GymStore.BuildingBlocks` provides a small, explicit, dependency-free CQRS mechanism:

```csharp
public interface ICommand<TResponse> { }
public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand<TResponse>
{ Task<TResponse> Handle(TCommand command, CancellationToken ct); }

public interface IQuery<TResponse> { }
public interface IQueryHandler<in TQuery, TResponse> where TQuery : IQuery<TResponse>
{ Task<TResponse> Handle(TQuery query, CancellationToken ct); }

public interface IDispatcher
{
    Task<TResponse> Send<TResponse>(ICommand<TResponse> command, CancellationToken ct = default);
    Task<TResponse> Query<TResponse>(IQuery<TResponse> query, CancellationToken ct = default);
}
```

- `AddCqrs()` registers the dispatcher; `AddHandlersFromAssembly(assembly)` scans a module's
  assembly and registers every `ICommandHandler<,>`/`IQueryHandler<,>`.
- **The dispatcher invokes handlers through the *public interface's* `Handle` method via
  cached reflection.** This is deliberate: it lets handler classes stay `internal` to their
  module. (An earlier `dynamic`-based version failed at runtime — the C# binder can't access
  an `internal` type's members across assemblies. Do not reintroduce `dynamic` here.)

**Why hand-rolled instead of MediatR:** MediatR v12+ is under a commercial license; a ~5-file
in-house dispatcher avoids that, keeps the mechanism transparent, and is trivially testable.
Cross-cutting concerns (validation, logging, transactions) can later be added as a pipeline
in this one place.

---

## 4. Inside the Cart module

```
Domain/            Cart, CartItem                       (module-owned entities; ids only, no
                                                          navigations to User/Product)
Application/
  Abstractions/    ICartRepository, ICartCache,
                   IProductInfoProvider (+ ProductInfo)
  Features/GetCart/     GetCartQuery + Handler
  Features/AddItem/     AddItemToCartCommand + Handler
  Features/RemoveItem/  RemoveItemFromCartCommand + Handler
  Responses/       CartResponse, CartItemResponse,       (HTTP response shape == old CartDto)
                   CartResponseFactory
Infrastructure/
  Persistence/     CartDbContext, CartRepository
  Caching/         DistributedCartCache                  (Redis, key "cart:{userId}")
  Public/          CartModuleApi : ICartModuleApi        (authoritative DB reads for other modules)
Api/               CartController                         (thin; dispatches commands/queries)
CartModuleExtensions.cs   AddCartModule(config) + Assembly handle for controller discovery
```

Request flow (unchanged externally):

```
HTTP → CartController → IDispatcher → Handler → ICartRepository (CartDbContext)
                                             └→ ICartCache (Redis, cache-aside)
                                             └→ IProductInfoProvider (enriches lines)
```

### Data ownership (transitional, zero-migration)

`CartDbContext` maps `Cart`/`CartItem` to the **existing** `Carts`/`CartItems` tables and
marks them `ExcludeFromMigrations()`. The module does all cart runtime reads/writes through
it. Meanwhile `GymStoreContext` **keeps** its Cart/CartItem mappings, so the schema, the FKs
to `Users`/`Products`, and the `User.Cart`/`Product.CartItems` navigations are untouched.

> Why keep both? Fully removing Cart from `GymStoreContext` would make EF scaffold a migration
> that **drops** the `Carts`/`CartItems` tables (because of the cross-entity FKs). Keeping the
> mappings there means **no migration and no risk** in this step. Ownership flips fully to
> `CartDbContext` once the adjacent modules (Users/Catalog) are extracted.

### Cart → Catalog dependency

Building the cart response needs product name/price/images. Cart depends on the
consumer-defined port `IProductInfoProvider`; the **host** implements it today
(`backend/Adapters/ProductInfoProvider.cs`, over `GymStoreContext.Products`). When a Catalog
module exists, the implementation moves there and Cart depends on `Catalog.Contracts` — Cart
never depends on Catalog internals.

---

## 5. Cross-module contract (Ordering → Cart)

`GymStore.Modules.Cart.Contracts` is the only thing other modules see:

```csharp
public interface ICartModuleApi
{
    Task<CartContentsDto?> GetCartAsync(long userId, CancellationToken ct = default);
    Task ClearCartAsync(long userId, CancellationToken ct = default);
}
public sealed record CartContentsDto(long CartId, long UserId, IReadOnlyList<CartLineDto> Items);
public sealed record CartLineDto(long ProductId, int Quantity);   // ids + quantities only
```

`OrderRepository.PlaceOrderAsync` was refactored to:

1. get cart contents via `ICartModuleApi.GetCartAsync` (no more direct table access);
2. resolve current unit prices from `Products` (Catalog data);
3. save the order;
4. clear the cart via `ICartModuleApi.ClearCartAsync` **after** the order commits (best-effort,
   logged on failure).

**Trade-off (accepted):** order-insert and cart-clear can no longer share one EF transaction
across the module boundary. A crash between them leaves an order placed but the cart
un-cleared — benign for a cart, and can be made exactly-once with an outbox when Ordering
becomes its own module.

---

## 6. Behavior preserved (and one fix)

Verified identical to before:

- Routes: `GET api/Cart/{userId}`, `POST api/Cart/{userId}/add/{productId}?quantity=`,
  `DELETE api/Cart/{userId}/remove/{productId}`.
- `GET` on a missing cart still creates+persists an empty cart and returns
  `{ "id", "userId", "items": [] }`.
- `POST add` **sets** the quantity to the passed value (the "+1" lives in the frontend).
- `DELETE` still returns `{ "message": "Product removed from cart" }`.
- Item JSON: `{ productId, productName, imageUrls, price, quantity, totalPrice }`.
- Placing an order with an empty cart still throws `"Cart is empty."`.

**Improvement:** checkout now **evicts the Redis cart cache** (`ClearCartAsync`). Previously
checkout deleted cart rows but left the cached cart stale until its TTL expired.

---

## 7. Tests

`GymStore.Modules.Cart.Tests` (xUnit, EF Core InMemory, hand-written fakes for cache and
product provider; the module exposes internals via `InternalsVisibleTo`):

- GetCart: empty → creates+caches; cache hit → returns cached instance.
- AddItem: new line enriched from catalog + cached; existing line → sets quantity (not +).
- RemoveItem: removes only the target line.
- CartModuleApi: ClearCart empties + evicts cache; GetCart returns ids/quantities.
- Dispatcher: routes command/query to registered handlers (incl. `internal` handlers).

All 9 pass.

---

## 8. How to build, test, and run

Prereqrequisites: .NET 9 SDK, SQL Server `(local)`, Redis (Podman:
`podman run -d --name gymstore-redis -p 6379:6379 redis:7`), a registered user (the first
user gets `Id = 1`, which the frontend's hardcoded cart uses).

```bash
# from backend/
dotnet build backend.sln
```
```bash
# tests (private NuGet feed is currently down, so restore from nuget.org)
dotnet test Modules/Cart/GymStore.Modules.Cart.Tests -s https://api.nuget.org/v3/index.json
```
```bash
# run the API (from backend/backend)
dotnet run
```

Smoke test:

```bash
curl -s http://localhost:5074/api/Cart/1
curl -s -X POST "http://localhost:5074/api/Cart/1/add/1?quantity=2"
curl -s -X DELETE "http://localhost:5074/api/Cart/1/remove/1"
```

> Note: the default NuGet source `PMS_NUGET_POLY_NEW` is currently unreachable; adding new
> packages needs `-s https://api.nuget.org/v3/index.json`. Normal builds work from cache.

---

## 9. Adding the next module (the template)

1. Create `Modules/<Name>/GymStore.Modules.<Name>.Contracts` (public API + DTOs, no deps).
2. Create `GymStore.Modules.<Name>` with `Domain / Application (CQRS features) / Infrastructure
   / Api`, its own `DbContext` (map existing tables with `ExcludeFromMigrations()` first), and
   an `Add<Name>Module(config)` extension that also calls `AddHandlersFromAssembly`.
3. Replace any direct cross-module data access with calls to the relevant `*.Contracts`
   interface (define a consumer-side port + host adapter if the provider module isn't extracted
   yet — as Cart does with `IProductInfoProvider`).
4. In the host: reference the projects, call `Add<Name>Module`, and
   `AddControllers().AddApplicationPart(<Name>ModuleExtensions.Assembly)`.
5. Add a `GymStore.Modules.<Name>.Tests` project.

**Suggested next:** Catalog — it lets us delete the host's `IProductInfoProvider` adapter and
give Cart a real `Catalog.Contracts` dependency, then flip `Carts`/`CartItems` DDL ownership
fully to `CartDbContext`.

---

## 10. Out of scope (future)

Extracting Catalog/Ordering/Payments; flipping full DB ownership to module contexts; an outbox
for order/cart consistency; committing the gitignored `Data/` folder; renaming the host project.
