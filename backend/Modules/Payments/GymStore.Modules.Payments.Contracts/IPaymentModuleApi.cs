namespace GymStore.Modules.Payments.Contracts;

/// <summary>
/// The Payments module's public surface for other modules (e.g. Ordering).
/// Callers depend on this contract only — never on Payments internals or its tables.
/// </summary>
public interface IPaymentModuleApi
{
    /// <summary>
    /// Records a payment for an order. Only method/amount are captured — never card data.
    /// </summary>
    Task CreatePaymentAsync(long orderId, string method, decimal amount, CancellationToken ct = default);
}
