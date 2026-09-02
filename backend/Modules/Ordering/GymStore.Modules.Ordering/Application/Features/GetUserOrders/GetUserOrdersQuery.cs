using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Ordering.Application.Responses;

namespace GymStore.Modules.Ordering.Application.Features.GetUserOrders;

public sealed record GetUserOrdersQuery(long UserId) : IQuery<IReadOnlyList<OrderResponse>>;
