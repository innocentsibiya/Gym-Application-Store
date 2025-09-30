using backend.Interfaces;
using backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.IO;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace backend.Services
{
    public class CartService : ICartService
    {
        private string _filePath = "Data/orders.json";  // Path to the JSON file where cart items are saved
        private readonly IMemoryCache _cache;

        public CartService(IMemoryCache cache)
        {
            _cache = cache;
        }

        // Helper method to read cart data from the JSON file
        public List<CartItems> ReadCartFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<CartItems>();  // If file doesn't exist, return an empty list
            }

            var json = File.ReadAllText(_filePath);
            var jToken = JToken.Parse(json);

            // Check if JSON is an array or object
            if (jToken.Type == JTokenType.Array)
            {
                // Deserialize as List<CartItem>
                return jToken.ToObject<List<CartItems>>() ?? new List<CartItems>();
            }
            else if (jToken.Type == JTokenType.Object)
            {
                // If it's a single CartItem, wrap it in a list
                var singleItem = jToken.ToObject<CartItems>();
                return new List<CartItems> { singleItem };
            }

            return new List<CartItems>();  // Fallback if the JSON is not valid
        }

        // Helper method to write cart data to the JSON file
        public void WriteCartToFile(List<CartItems> cartItems)
        {
            var json = JsonConvert.SerializeObject(cartItems, Formatting.Indented);
            File.WriteAllText(_filePath, json);
        }

        // Get the cart items
        public List<CartItems> GetCart()
        {
            if(!_cache.TryGetValue("CartCache", out List<CartItems> cart))
            {
                cart = ReadCartFromFile();
                _cache.Set("CartCache",cart,TimeSpan.FromMinutes(15));
            }
            return cart ?? [];
        }

        // Add an item to the cart
        public List<CartItems> AddToCart(CartItems newItem)
        {
            var cartItems = GetCart();

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
            //save to the cache
            _cache.Set("CartCache", cartItems, TimeSpan.FromMinutes(15));
            return cartItems;
        }

        // Remove an item from the cart
        public List<CartItems> RemoveFromCart(int id)
        {
            var cartItems = GetCart();
            var itemToRemove = cartItems.FirstOrDefault(item => item.Id == id);

            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
                WriteCartToFile(cartItems);  // Save the updated cart to file
            }
            //save to the cache
            _cache.Set("CartCache", cartItems, TimeSpan.FromMinutes(15));

            return cartItems;
        }
    }
}
