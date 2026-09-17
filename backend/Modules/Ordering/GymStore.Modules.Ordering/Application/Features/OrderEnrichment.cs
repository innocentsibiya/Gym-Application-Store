using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Application.Responses;
using GymStore.Modules.Ordering.Domain;

namespace GymStore.Modules.Ordering.Application.Features;

/// <summary>
/// Shared read-side mapping: turns orders into responses, enriching every line's product from
/// Catalog in a single batch lookup. Used by the order-history queries.
/// </summary>
internal static class OrderEnrichment
{
    public static async Task<IReadOnlyList<OrderResponse>> EnrichAsync(
        IReadOnlyList<Order> orders, ICatalogModuleApi catalog, CancellationToken ct)
    {
        var productIds = orders.SelectMany(o => o.Items).Select(i => i.ProductId).Distinct().ToList();

        IReadOnlyDictionary<long, ProductSummaryDto> products = productIds.Count == 0
            ? new Dictionary<long, ProductSummaryDto>()
            : await catalog.GetProductsByIdsAsync(productIds, ct);

        return orders.Select(o => OrderResponseFactory.ToResponse(o, products)).ToList();
    }
}
