using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Application.Responses;

namespace GymStore.Modules.Catalog.Application.Features.GetProductById;

/// <summary>
/// Cache-aside read of a single product. Throws <see cref="KeyNotFoundException"/> when the
/// product does not exist, preserving the original ProductService behavior.
/// </summary>
internal sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _repository;
    private readonly IProductCache _cache;

    public GetProductByIdQueryHandler(IProductRepository repository, IProductCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<ProductResponse> Handle(GetProductByIdQuery query, CancellationToken ct)
    {
        var cached = await _cache.GetByIdAsync(query.Id, ct);
        if (cached is not null)
        {
            return cached;
        }

        var product = await _repository.GetByIdAsync(query.Id, ct);
        if (product is null)
        {
            throw new KeyNotFoundException($"Product with ID {query.Id} not found.");
        }

        var response = ProductResponseFactory.ToResponse(product);
        await _cache.SetByIdAsync(query.Id, response, ct);
        return response;
    }
}
