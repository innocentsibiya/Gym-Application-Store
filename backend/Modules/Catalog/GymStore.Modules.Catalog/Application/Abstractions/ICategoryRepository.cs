using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Abstractions;

/// <summary>Read operations for categories (module-internal).</summary>
public interface ICategoryRepository
{
    /// <summary>All categories with their sub-categories loaded.</summary>
    Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct);

    /// <summary>A category with its products and sub-categories, or null.</summary>
    Task<Category?> GetWithProductsAsync(long id, CancellationToken ct);
}
