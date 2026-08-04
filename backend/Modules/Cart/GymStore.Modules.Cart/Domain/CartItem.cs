namespace GymStore.Modules.Cart.Domain;

/// <summary>A line in a <see cref="Cart"/>. References a product by id only.</summary>
public class CartItem
{
    public long Id { get; set; }
    public long CartId { get; set; }
    public long ProductId { get; set; }
    public int Quantity { get; set; }

    /// <summary>
    /// Persisted price column (kept for schema parity). The price shown to users is
    /// resolved from the Catalog at read time, mirroring the pre-refactor behavior.
    /// </summary>
    public decimal Price { get; set; }
}
