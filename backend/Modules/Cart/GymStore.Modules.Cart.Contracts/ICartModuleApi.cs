namespace GymStore.Modules.Cart.Contracts;

/// <summary>
/// The Cart module's public surface for other modules (e.g. Ordering).
/// Callers depend on this contract only — never on Cart internals or its tables.
/// </summary>
public interface ICartModuleApi
{
    /// <summary>Returns the user's cart contents, or <c>null</c> if the user has no cart.</summary>
    Task<CartContentsDto?> GetCartAsync(long userId, CancellationToken ct = default);

    /// <summary>Removes all items from the user's cart and invalidates any cached copy.</summary>
    Task ClearCartAsync(long userId, CancellationToken ct = default);
}
