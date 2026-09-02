using GymStore.Modules.Payments.Domain;

namespace GymStore.Modules.Payments.Application.Abstractions;

/// <summary>Persistence operations for payments (module-internal).</summary>
public interface IPaymentRepository
{
    Task<Payment> AddAsync(Payment payment, CancellationToken ct);
    Task<Payment?> GetByOrderIdAsync(long orderId, CancellationToken ct);
}
