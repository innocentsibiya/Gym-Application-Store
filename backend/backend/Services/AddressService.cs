using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(long userId)
        {
            return await _addressRepository.GetUserAddressesAsync(userId);
        }

        public async Task<Address?> GetDefaultAddressAsync(long userId, string type)
        {
            return await _addressRepository.GetDefaultAddressAsync(userId, type);
        }

        public async Task AddAddressAsync(Address address)
        {
            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();
        }

        public async Task RemoveAddressAsync(long id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address != null)
            {
                _addressRepository.Remove(address);
                await _addressRepository.SaveChangesAsync();
            }
        }
    }
}