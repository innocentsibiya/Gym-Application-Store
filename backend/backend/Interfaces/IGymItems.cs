using backend.Models;

namespace backend.Interfaces
{
    public interface IGymItems
    {
        List<GymItem> GetAllItems();
        GymItem? GetItemById(int id);
        List<GymItem> GetItemsByCategory(string category);
        void CreateItem(GymItem newItem);
    }
}
