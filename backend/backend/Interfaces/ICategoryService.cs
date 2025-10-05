using backend.Models;

namespace backend.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<Category>> GetAllCategoriesAsync();
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);
    }
}