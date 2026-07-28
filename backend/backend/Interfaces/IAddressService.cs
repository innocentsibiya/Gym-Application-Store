using backend.DTO;

namespace backend.Interfaces
{
    public interface IAddressService
    {
        Task<IEnumerable<AddressDto>> GetUserAddressesAsync(long userId);
        Task<AddressDto?> GetDefaultAddressAsync(long userId, string type);
        Task AddAddressAsync(AddressDto address);

        Task UpdateAddressAsync(AddressDto dto);
        Task RemoveAddressAsync(long id);
    }
}