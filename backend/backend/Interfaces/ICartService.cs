using backend.Models;

namespace backend.Interfaces
{
    public interface ICartService
    {
        // Get the cart items
        List<CartItem> GetCart();

        // Add an item to the cart
        List<CartItem> AddToCart(CartItem newItem);

        // Remove an item from the cart
        List<CartItem> RemoveFromCart(int id);
    }
}
