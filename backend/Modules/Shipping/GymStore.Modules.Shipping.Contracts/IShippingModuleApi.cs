namespace GymStore.Modules.Shipping.Contracts;

/// <summary>
/// The Shipping module's public surface for other modules (e.g. Ordering).
/// Callers depend on this contract only — never on Shipping internals or its tables.
/// </summary>
public interface IShippingModuleApi
{
    /// <summary>Creates a pending shipment for an order (called at checkout).</summary>
    Task CreateShipmentAsync(long orderId, CancellationToken ct = default);
}
