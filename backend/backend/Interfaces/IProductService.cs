using backend.DTO;
using backend.Models;

namespace backend.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProductsAsync();
        Task<ProductDto?> GetProductByIdAsync(long id);
    }
}