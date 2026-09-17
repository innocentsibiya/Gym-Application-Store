using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Payments.Application.Abstractions;
using GymStore.Modules.Payments.Application.Responses;

namespace GymStore.Modules.Payments.Application.Features.GetPaymentByOrder;

internal sealed class GetPaymentByOrderQueryHandler : IQueryHandler<GetPaymentByOrderQuery, PaymentResponse?>
{
    private readonly IPaymentRepository _repository;

    public GetPaymentByOrderQueryHandler(IPaymentRepository repository) => _repository = repository;

    public async Task<PaymentResponse?> Handle(GetPaymentByOrderQuery query, CancellationToken ct)
    {
        var payment = await _repository.GetByOrderIdAsync(query.OrderId, ct);
        return payment is null ? null : PaymentResponseFactory.ToResponse(payment);
    }
}
