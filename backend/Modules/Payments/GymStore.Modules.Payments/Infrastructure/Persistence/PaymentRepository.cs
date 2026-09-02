using GymStore.Modules.Payments.Application.Abstractions;
using GymStore.Modules.Payments.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Payments.Infrastructure.Persistence;

internal sealed class PaymentRepository : IPaymentRepository
{
    private readonly PaymentDbContext _db;

    public PaymentRepository(PaymentDbContext db) => _db = db;

    public async Task<Payment> AddAsync(Payment payment, CancellationToken ct)
    {
        _db.Payments.Add(payment);
        await _db.SaveChangesAsync(ct);
        return payment;
    }

    public Task<Payment?> GetByOrderIdAsync(long orderId, CancellationToken ct) =>
        _db.Payments.AsNoTracking().FirstOrDefaultAsync(p => p.OrderId == orderId, ct);
}
