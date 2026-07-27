using backend.DTO;
using backend.Models;

namespace backend.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(long id);
        Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string term, int page, int pageSize);
    }
}