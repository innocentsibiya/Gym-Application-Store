using backend.DTO;
using backend.Models;

namespace backend.IRepository
{
    public interface ICartRepository : IRepository<Cart>
    {
        Task<CartDto?> GetCartByUserIdAsync(int userId);
        Task<CartDto> AddItemAsync(int userId, int productId, int quantity);
        Task<CartDto> RemoveItemAsync(int userId, int productId);
    }
}