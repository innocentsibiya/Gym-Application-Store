using backend.Adapters;
using backend.Data;
using backend.Interfaces;
using backend.IRepository;
using backend.Repository;
using backend.Services;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common;
using GymStore.Common.Modules;
using GymStore.Modules.Addresses;
using GymStore.Modules.Cart;
using GymStore.Modules.Cart.Application.Abstractions;
using GymStore.Modules.Catalog;
using GymStore.Modules.Identity;
using GymStore.Modules.Inventory;
using GymStore.Modules.Ordering;
using GymStore.Modules.Payments;
using GymStore.Modules.Preferences;
using GymStore.Modules.Reviews;
using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Shipping;
using GymStore.Modules.Suppliers;
using GymStore.Modules.Wishlist;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});

// The modular-monolith modules, registered uniformly via the common IModule mechanism.
var modules = new IModule[]
{
    new CartModule(),
    new CatalogModule(),
    new OrderingModule(),
    new PaymentsModule(),
    new ShippingModule(),
    new ReviewsModule(),
    new InventoryModule(),
    new SuppliersModule(),
    new WishlistModule(),
    new AddressesModule(),
    new IdentityModule(),
    new PreferencesModule()
};

// Add services to the container.
var mvcBuilder = builder.Services.AddControllers()
    .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
foreach (var module in modules)
{
    // Discover each module's controllers.
    mvcBuilder.AddApplicationPart(module.Assembly);
}

// Shared kernel + CQRS dispatcher, then each module's own services.
builder.Services.AddCommon();
builder.Services.AddCqrs();
foreach (var module in modules)
{
    module.Register(builder.Services, builder.Configuration);
}

// Host adapters that satisfy module ports.
// Lets the Cart module read product data (name/price/images) from Catalog.
builder.Services.AddScoped<IProductInfoProvider, ProductInfoProvider>();
// Lets the Reviews module resolve reviewer display names.
builder.Services.AddScoped<IReviewerInfoProvider, ReviewerInfoProvider>();

builder.Services.AddScoped<ICouponService, CouponService>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<ICouponRepository, CouponRepository>();

// JWT bearer authentication is configured by the Identity module (single source for the key);
// authorization stays a host concern.
builder.Services.AddAuthorization();

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration =
        builder.Configuration.GetConnectionString("Redis");

    options.InstanceName = "GymStore:";
});
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
// Add Swagger/OpenAPI services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<GymStoreContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"))
                    // The reconstructed DbContext intentionally omits the HasData seed
                    // (the seed is applied by the existing migration's InsertData calls),
                    // so ignore the resulting data-only pending-model-changes warning.
                    .ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning)));

var app = builder.Build();
app.UseCors("AllowAll");
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable middleware to serve generated Swagger as a JSON endpoint
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
        c.RoutePrefix = string.Empty; // To serve the Swagger UI at application's root (e.g., http://localhost:<port>/)
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();

app.MapControllers();

app.Run();
