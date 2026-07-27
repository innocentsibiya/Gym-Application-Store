using backend.Models;

namespace backend.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<Address>> GetUserAddressesAsync(long userId);
        Task<Address?> GetDefaultAddressAsync(long userId, string type);
        Task AddAddressAsync(Address address);
        Task RemoveAddressAsync(long id);
    }
}