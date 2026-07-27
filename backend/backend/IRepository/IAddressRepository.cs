using backend.Models;

namespace backend.IRepository
{
    public interface IAddressRepository : IRepository<Address>
    {
        Task<IEnumerable<Address>> GetUserAddressesAsync(long userId);
        Task<Address?> GetDefaultAddressAsync(long userId, string type);
    }
}