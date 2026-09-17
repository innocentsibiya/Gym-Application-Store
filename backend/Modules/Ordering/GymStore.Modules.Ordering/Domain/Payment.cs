namespace GymStore.Modules.Ordering.Domain;

/// <summary>Payment record for an order (part of the aggregate; not yet exercised by any flow).</summary>
public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string PaymentMethod { get; set; } = "CreditCard";
    public string PaymentStatus { get; set; } = "Pending";
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
