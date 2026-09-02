using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Features.SearchProducts;

public sealed record SearchProductsQuery(string Term, int Page, int PageSize)
    : IQuery<SearchProductsResult>;

/// <summary>A page of matching products plus the total match count.</summary>
public sealed record SearchProductsResult(IReadOnlyList<Product> Products, int TotalCount);
