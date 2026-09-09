namespace GymStore.Modules.Suppliers.Domain;

/// <summary>
/// A supplier, owned by the Suppliers module. Holds only supplier data — the inventory it
/// supplies is owned by the Inventory module and is not part of this aggregate.
/// </summary>
public class Supplier
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
