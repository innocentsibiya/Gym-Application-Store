using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Application.Responses;

namespace GymStore.Modules.Catalog.Application.Features.GetAllProducts;

/// <summary>Cache-aside list of active products (mirrors the original ProductService.GetProductsAsync).</summary>
internal sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IReadOnlyList<ProductResponse>>
{
    private readonly IProductRepository _repository;
    private readonly IProductCache _cache;

    public GetAllProductsQueryHandler(IProductRepository repository, IProductCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<IReadOnlyList<ProductResponse>> Handle(GetAllProductsQuery query, CancellationToken ct)
    {
        var cached = await _cache.GetAllAsync(ct);
        if (cached is not null)
        {
            return cached;
        }

        var products = await _repository.GetActiveAsync(ct);
        var response = products.Select(ProductResponseFactory.ToResponse).ToList();

        await _cache.SetAllAsync(response, ct);
        return response;
    }
}
