using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Catalog.Infrastructure.Persistence;

internal sealed class CategoryRepository : ICategoryRepository
{
    private readonly CatalogDbContext _db;

    public CategoryRepository(CatalogDbContext db) => _db = db;

    public async Task<IReadOnlyList<Category>> GetAllAsync(CancellationToken ct) =>
        await _db.Categories
            .Include(c => c.SubCategories)
            .ToListAsync(ct);

    public Task<Category?> GetWithProductsAsync(long id, CancellationToken ct) =>
        _db.Categories
            .Include(c => c.Products)
            .Include(c => c.SubCategories)
            .FirstOrDefaultAsync(c => c.Id == id, ct);
}
