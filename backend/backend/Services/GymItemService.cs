using backend.Interfaces;
using backend.Models;
using System.Text.Json;

namespace backend.Services
{
    public class GymItemService : IGymItems
    {
        private readonly string _dataFilePath = "Data/gym_equipment.json";
        public List<GymItem> GetAllItems()
        {
            if (!File.Exists(_dataFilePath))
                return new List<GymItem>();

            string json = File.ReadAllText(_dataFilePath);

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            List<GymItem>? items = JsonSerializer.Deserialize<List<GymItem>>(json, options);

            return items ?? new List<GymItem>();
        }

        public GymItem? GetItemById(int id)
        {
            var items = GetAllItems();
            return items.FirstOrDefault(item => item.Id == id);
        }

        public List<GymItem> GetItemsByCategory(string category)
        {
            var items = GetAllItems();
            return items
                .Where(item => item.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public void CreateItem(GymItem newItem)
        {
            var items = GetAllItems();

            int newId = (items.Any() ? items.Max(i => i.Id) : 0) + 1;
            newItem.Id = newId;

            items.Add(newItem);

            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_dataFilePath, json);
        }
    }
}
