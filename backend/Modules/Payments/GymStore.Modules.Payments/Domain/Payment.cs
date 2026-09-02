namespace GymStore.Modules.Payments.Domain;

/// <summary>
/// A payment recorded against an order. Owned by the Payments module. Deliberately holds no
/// card data — only method, status, amount and a transaction reference.
/// </summary>
public class Payment
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public string PaymentMethod { get; set; } = "Card";
    public string PaymentStatus { get; set; } = "Pending";
    public string? TransactionId { get; set; }
    public decimal Amount { get; set; }
    public DateTime? PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
