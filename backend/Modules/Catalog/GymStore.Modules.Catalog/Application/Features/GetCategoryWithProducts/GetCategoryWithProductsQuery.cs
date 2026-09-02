using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Features.GetCategoryWithProducts;

public sealed record GetCategoryWithProductsQuery(long Id) : IQuery<Category?>;
