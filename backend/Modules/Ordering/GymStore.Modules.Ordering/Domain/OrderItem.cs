namespace GymStore.Modules.Ordering.Domain;

/// <summary>A line in an <see cref="Order"/>. References a product by id only.</summary>
public class OrderItem
{
    public long Id { get; set; }
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal? DiscountApplied { get; set; }
}
