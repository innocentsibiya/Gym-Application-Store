using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Contracts;
using GymStore.Modules.Ordering.Application.Abstractions;
using GymStore.Modules.Ordering.Application.Features;
using GymStore.Modules.Ordering.Application.Responses;

namespace GymStore.Modules.Ordering.Application.Features.GetUserOrdersByYear;

internal sealed class GetUserOrdersByYearQueryHandler
    : IQueryHandler<GetUserOrdersByYearQuery, IReadOnlyList<OrderResponse>>
{
    private readonly IOrderRepository _orders;
    private readonly ICatalogModuleApi _catalog;

    public GetUserOrdersByYearQueryHandler(IOrderRepository orders, ICatalogModuleApi catalog)
    {
        _orders = orders;
        _catalog = catalog;
    }

    public async Task<IReadOnlyList<OrderResponse>> Handle(GetUserOrdersByYearQuery query, CancellationToken ct)
    {
        var orders = await _orders.GetByUserAndYearAsync(query.UserId, query.Year, ct);
        return await OrderEnrichment.EnrichAsync(orders, _catalog, ct);
    }
}
