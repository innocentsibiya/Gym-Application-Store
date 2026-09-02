using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Payments.Application.Responses;

namespace GymStore.Modules.Payments.Application.Features.GetPaymentByOrder;

public sealed record GetPaymentByOrderQuery(long OrderId) : IQuery<PaymentResponse?>;
