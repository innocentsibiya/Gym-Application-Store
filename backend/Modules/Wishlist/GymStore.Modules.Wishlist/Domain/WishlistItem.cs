namespace GymStore.Modules.Wishlist.Domain;

/// <summary>An item in a <see cref="Wishlist"/>. References a product by id only.</summary>
public class WishlistItem
{
    public long Id { get; set; }
    public long WishlistId { get; set; }
    public long ProductId { get; set; }
}
