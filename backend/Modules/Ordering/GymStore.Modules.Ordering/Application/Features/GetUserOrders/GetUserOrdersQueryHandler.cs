using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Application.Features;
using GymStore.Modules.Ordering.Application.Responses;

namespace GymStore.Modules.Ordering.Application.Features.GetUserOrders;

internal sealed class GetUserOrdersQueryHandler : IQueryHandler<GetUserOrdersQuery, IReadOnlyList<OrderResponse>>
{
    private readonly IOrderRepository _orders;
    private readonly ICatalogModuleApi _catalog;

    public GetUserOrdersQueryHandler(IOrderRepository orders, ICatalogModuleApi catalog)
    {
        _orders = orders;
        _catalog = catalog;
    }

    public async Task<IReadOnlyList<OrderResponse>> Handle(GetUserOrdersQuery query, CancellationToken ct)
    {
        var orders = await _orders.GetByUserAsync(query.UserId, ct);
        return await OrderEnrichment.EnrichAsync(orders, _catalog, ct);
    }
}
