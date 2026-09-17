using GymStore.Modules.Payments.Domain;

namespace GymStore.Modules.Payments.Application.Responses;

/// <summary>HTTP response shape for a payment.</summary>
public sealed class PaymentResponse
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string PaymentStatus { get; set; } = string.Empty;
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; }
}

internal static class PaymentResponseFactory
{
    public static PaymentResponse ToResponse(Payment p) => new()
    {
        Id = p.Id,
        OrderId = p.OrderId,
        PaymentMethod = p.PaymentMethod,
        PaymentStatus = p.PaymentStatus,
        TransactionId = p.TransactionId,
        Amount = p.Amount,
        PaidAt = p.PaidAt,
        CreatedAt = p.CreatedAt
    };
}
