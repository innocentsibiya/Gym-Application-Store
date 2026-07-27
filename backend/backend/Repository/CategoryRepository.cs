using backend.Data;
using backend.Interfaces;
using backend.IRepository;
using backend.Models;
using backend.Repository;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly GymStoreContext _context;

        public CategoryRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories
                .Include(c => c.SubCategories)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
        {
            return await _context.Categories
                .Include(c => c.Products)
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}