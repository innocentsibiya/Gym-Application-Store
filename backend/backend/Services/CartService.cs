using backend.Interfaces;
using backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
using System.Collections.Generic;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private string _filePath = "Data/orders.json";  // Path to the JSON file where cart items are saved

        // Helper method to read cart data from the JSON file
        public List<CartItem> ReadCartFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<CartItem>();  // If file doesn't exist, return an empty list
            }

            var json = File.ReadAllText(_filePath);
            var jToken = JToken.Parse(json);

            // Check if JSON is an array or object
            if (jToken.Type == JTokenType.Array)
            {
                // Deserialize as List<CartItem>
                return jToken.ToObject<List<CartItem>>() ?? new List<CartItem>();
            }
            else if (jToken.Type == JTokenType.Object)
            {
                // If it's a single CartItem, wrap it in a list
                var singleItem = jToken.ToObject<CartItem>();
                return new List<CartItem> { singleItem };
            }

            return new List<CartItem>();  // Fallback if the JSON is not valid
        }

        // Helper method to write cart data to the JSON file
        public void WriteCartToFile(List<CartItem> cartItems)
        {
            var json = JsonConvert.SerializeObject(cartItems, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        // Get the cart items
        public List<CartItem> GetCart()
        {
            return ReadCartFromFile();
        }

        // Add an item to the cart
        public List<CartItem> AddToCart(CartItem newItem)
        {
            var cartItems = ReadCartFromFile();

            // Check if the item is already in the cart
            var existingItem = cartItems.FirstOrDefault(item => item.Id == newItem.Id);
            if (existingItem != null)
            {
                existingItem.Quantity = newItem.Quantity;  // Update quantity if the item exists
            }
            else
            {
                cartItems.Add(newItem);  // Add new item to the cart
            }

            WriteCartToFile(cartItems);  // Save the updated cart to file
            return cartItems;
        }

        // Remove an item from the cart
        public List<CartItem> RemoveFromCart(int id)
        {
            var cartItems = ReadCartFromFile();
            var itemToRemove = cartItems.FirstOrDefault(item => item.Id == id);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                WriteCartToFile(cartItems);  // Save the updated cart to file
            }

            return cartItems;
        }
    }
}
