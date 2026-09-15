using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Responses;

namespace GymStore.Modules.Catalog.Application.Features.GetAllProducts;

public sealed record GetAllProductsQuery : IQuery<IReadOnlyList<ProductResponse>>;
