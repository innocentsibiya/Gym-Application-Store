using backend.DTO;
using backend.Models;

namespace backend.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetCartAsync(int userId);
        Task<CartDto> AddToCartAsync(int userId, int productId, int quantity);
        Task<CartDto> RemoveFromCartAsync(int userId, int productId);
    }
}