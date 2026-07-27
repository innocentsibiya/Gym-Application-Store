using backend.IRepository;
using backend.Models;

namespace backend.IRepository
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);
    }
}