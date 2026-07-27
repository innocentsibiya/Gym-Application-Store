using backend.Data;
using backend.DTO;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly GymStoreContext _context;

        public ProductRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ProductDto>> GetProductsAsync()
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Brand = p.Brand,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    SKU = p.SKU,
                    StockQuantity = p.StockQuantity,
                    Weight = p.Weight,
                    Dimensions = p.Dimensions,
                    IsActive = p.IsActive,
                    ImageUrls = p.Images.Select(img => img.ImageUrl).ToList()
                })
                .ToListAsync();
        }

        public async Task<ProductDto?> GetProductByIdAsync(long id)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.Id == id && p.IsActive)
                .Include(p => p.Category)
                .Include(p => p.Images)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    CategoryId = p.CategoryId,
                    CategoryName = p.Category.Name,
                    Name = p.Name,
                    Slug = p.Slug,
                    Description = p.Description,
                    Brand = p.Brand,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    SKU = p.SKU,
                    StockQuantity = p.StockQuantity,
                    Weight = p.Weight,
                    Dimensions = p.Dimensions,
                    IsActive = p.IsActive,
                    ImageUrls = p.Images.Select(img => img.ImageUrl).ToList()
                })
                .FirstOrDefaultAsync();
        }

        public async Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string term, int page, int pageSize)
        {
            IQueryable<Product> query = _context.Products
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

            var totalCount = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }
    }
}