namespace GymStore.Modules.Catalog.Contracts;

/// <summary>
/// The Catalog module's public surface for other modules (e.g. Cart, Ordering).
/// Callers depend on this contract only — never on Catalog internals or its tables.
/// </summary>
public interface ICatalogModuleApi
{
    /// <summary>
    /// Looks up summary data (name, price, image urls) for the given product ids.
    /// Missing ids are simply absent from the result.
    /// </summary>
    Task<IReadOnlyDictionary<long, ProductSummaryDto>> GetProductsByIdsAsync(
        IReadOnlyCollection<long> productIds, CancellationToken ct = default);
}

/// <summary>Minimal product projection other modules are allowed to consume.</summary>
public sealed record ProductSummaryDto(long ProductId, string Name, decimal Price, IReadOnlyList<string> ImageUrls);
