using backend.Models;

namespace backend.Interfaces
{
    public interface ICartService
    {
        // Get the cart items
        List<CartItems> GetCart();

        // Add an item to the cart
        List<CartItems> AddToCart(CartItems newItem);

        // Remove an item from the cart
        List<CartItems> RemoveFromCart(int id);
    }
}
