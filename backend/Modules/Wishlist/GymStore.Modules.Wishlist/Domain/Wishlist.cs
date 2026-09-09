namespace GymStore.Modules.Wishlist.Domain;

/// <summary>
/// The Wishlist aggregate root, owned by the Wishlist module. Holds the owning user's id and its
/// items; product details are resolved from the Catalog at read time.
/// </summary>
public class Wishlist
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public List<WishlistItem> Items { get; set; } = new();
}
