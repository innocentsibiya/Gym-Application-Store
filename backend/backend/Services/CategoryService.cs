using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _categoryRepository.GetAllCategoriesAsync();
        }

        public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
        {
            return await _categoryRepository.GetCategoryWithProductsAsync(categoryId);
        }
    }
}