using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Wishlist.Application.Abstractions;
using GymStore.Modules.Wishlist.Application.Responses;

namespace GymStore.Modules.Wishlist.Application.Features.GetWishlist;

/// <summary>
/// Loads (or lazily creates) the user's wishlist and enriches each item with product data from
/// the Catalog, mirroring the original auto-create behavior.
/// </summary>
internal sealed class GetWishlistQueryHandler : IQueryHandler<GetWishlistQuery, WishlistResponse>
{
    private readonly IWishlistRepository _repository;
    private readonly ICatalogModuleApi _catalog;

    public GetWishlistQueryHandler(IWishlistRepository repository, ICatalogModuleApi catalog)
    {
        _repository = repository;
        _catalog = catalog;
    }

    public async Task<WishlistResponse> Handle(GetWishlistQuery query, CancellationToken ct)
    {
        var wishlist = await _repository.GetByUserIdAsync(query.UserId, ct)
                       ?? await _repository.CreateAsync(query.UserId, ct);

        var productIds = wishlist.Items.Select(i => i.ProductId).Distinct().ToArray();
        var products = productIds.Length == 0
            ? new Dictionary<long, ProductSummaryDto>()
            : await _catalog.GetProductsByIdsAsync(productIds, ct);

        return WishlistResponseFactory.Create(wishlist, products);
    }
}
