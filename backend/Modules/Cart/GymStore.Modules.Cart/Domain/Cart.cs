namespace GymStore.Modules.Cart.Domain;

/// <summary>
/// The Cart aggregate root. Owned entirely by the Cart module — it deliberately has no
/// navigation to User or Product (those belong to other modules); it holds only their ids.
/// </summary>
public class Cart
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    public List<CartItem> Items { get; set; } = new();
}
