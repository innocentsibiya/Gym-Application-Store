using GymStore.Modules.Payments.Application.Abstractions;
using GymStore.Modules.Payments.Contracts;
using GymStore.Modules.Payments.Domain;

namespace GymStore.Modules.Payments.Infrastructure.Public;

/// <summary>
/// Implements the cross-module contract. Records a (simulated) captured payment for an order —
/// no gateway call and no card data, only method/amount/status/transaction reference.
/// </summary>
internal sealed class PaymentModuleApi : IPaymentModuleApi
{
    private readonly IPaymentRepository _repository;

    public PaymentModuleApi(IPaymentRepository repository) => _repository = repository;

    public async Task CreatePaymentAsync(long orderId, string method, decimal amount, CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;
        var payment = new Payment
        {
            OrderId = orderId,
            PaymentMethod = string.IsNullOrWhiteSpace(method) ? "Card" : method,
            Amount = amount,
            PaymentStatus = "Paid",
            PaidAt = now,
            CreatedAt = now,
            TransactionId = "TXN-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant()
        };

        await _repository.AddAsync(payment, ct);
    }
}
