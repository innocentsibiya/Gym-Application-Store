using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Domain;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Catalog.Infrastructure.Persistence;

internal sealed class ProductRepository : IProductRepository
{
    private readonly CatalogDbContext _db;

    public ProductRepository(CatalogDbContext db) => _db = db;

    public async Task<IReadOnlyList<Product>> GetActiveAsync(CancellationToken ct) =>
        await _db.Products
            .AsNoTracking()
            .Where(p => p.IsActive)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .ToListAsync(ct);

    public Task<Product?> GetByIdAsync(long id, CancellationToken ct) =>
        _db.Products
            .AsNoTracking()
            .Where(p => p.Id == id && p.IsActive)
            .Include(p => p.Category)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(ct);

    public async Task<(IReadOnlyList<Product> Products, int TotalCount)> SearchAsync(
        string term, int page, int pageSize, CancellationToken ct)
    {
        IQueryable<Product> query = _db.Products
            .AsNoTracking()
            .Include(p => p.Category)
            .Include(p => p.Images)
            .Where(p => p.IsActive);

        if (!string.IsNullOrWhiteSpace(term))
        {
            term = term.ToLower();
            query = query.Where(p =>
                p.Name.ToLower().Contains(term) ||
                p.Description.ToLower().Contains(term) ||
                (p.Brand != null && p.Brand.ToLower().Contains(term)) ||
                (p.Category != null && p.Category.Name.ToLower().Contains(term)));
        }

        var totalCount = await query.CountAsync(ct);
        var products = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return (products, totalCount);
    }

    public async Task<IReadOnlyList<Product>> GetByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken ct) =>
        await _db.Products
            .AsNoTracking()
            .Where(p => ids.Contains(p.Id))
            .Include(p => p.Images)
            .ToListAsync(ct);
}
