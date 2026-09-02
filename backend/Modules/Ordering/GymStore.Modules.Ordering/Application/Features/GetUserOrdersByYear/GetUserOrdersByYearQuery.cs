using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Ordering.Application.Responses;

namespace GymStore.Modules.Ordering.Application.Features.GetUserOrdersByYear;

public sealed record GetUserOrdersByYearQuery(long UserId, int Year) : IQuery<IReadOnlyList<OrderResponse>>;
