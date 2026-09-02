using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Abstractions;

namespace GymStore.Modules.Catalog.Application.Features.SearchProducts;

internal sealed class SearchProductsQueryHandler : IQueryHandler<SearchProductsQuery, SearchProductsResult>
{
    private readonly IProductRepository _repository;

    public SearchProductsQueryHandler(IProductRepository repository) => _repository = repository;

    public async Task<SearchProductsResult> Handle(SearchProductsQuery query, CancellationToken ct)
    {
        var (products, totalCount) = await _repository.SearchAsync(query.Term, query.Page, query.PageSize, ct);
        return new SearchProductsResult(products, totalCount);
    }
}
