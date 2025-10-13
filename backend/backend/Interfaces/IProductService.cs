using backend.DTO;
using backend.Models;

namespace backend.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(long id);

        Task<(IEnumerable<Product> Products, int TotalCount)> SearchAsync(string term, int page, int pageSize);
    }
}