namespace GymStore.Modules.Cart.Application.Abstractions;

/// <summary>
/// Port the Cart module uses to enrich its lines with product data (name, price, images).
/// Consumer-defined interface (DIP): the host implements it over the Catalog today, and a
/// future Catalog module will implement it via its own contract — Cart never depends on
/// Catalog internals.
/// </summary>
public interface IProductInfoProvider
{
    Task<IReadOnlyDictionary<long, ProductInfo>> GetProductsAsync(
        IReadOnlyCollection<long> productIds, CancellationToken ct);
}

public sealed record ProductInfo(long ProductId, string Name, decimal Price, IReadOnlyList<string> ImageUrls);
