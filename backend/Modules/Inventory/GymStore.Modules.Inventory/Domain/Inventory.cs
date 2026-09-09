namespace GymStore.Modules.Inventory.Domain;

/// <summary>
/// Stock record for a product, owned by the Inventory module. References the product (and an
/// optional supplier) by id only — other modules own those concerns.
/// </summary>
public class Inventory
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public int QuantityAvailable { get; set; }
    public int ReorderLevel { get; set; }
    public long? SupplierId { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
}
