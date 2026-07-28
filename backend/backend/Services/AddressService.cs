using backend.DTO;
using backend.Interfaces;
using backend.IRepository;
using backend.Models;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace backend.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IDistributedCache _cache;
        private const string CacheKeyPrefix = "addresses:";

        public AddressService(IAddressRepository addressRepository, IDistributedCache cache)
        {
            _addressRepository = addressRepository;
            _cache = cache;
        }

        private static string GetKey(long userId) => $"{CacheKeyPrefix}{userId}";

        public async Task<IEnumerable<AddressDto>> GetUserAddressesAsync(long userId)
        {
            var cacheKey = GetKey(userId);
            var cached = await _cache.GetStringAsync(cacheKey);

            if (!string.IsNullOrEmpty(cached))
                return JsonSerializer.Deserialize<IEnumerable<AddressDto>>(cached)!;

            var addresses = await _addressRepository.GetUserAddressesAsync(userId);
            var returnAddresses = addresses.Select(a => new AddressDto
            {
                Id = a.Id,
                UserId = a.UserId,
                Street = a.Street,
                City = a.City,
                Province = a.Province,
                PostalCode = a.PostalCode,
                Country = a.Country,
                AddressType = a.AddressType,
                IsDefault = a.IsDefault
            }).ToList();

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(returnAddresses), options);

            return returnAddresses;
        }

        public async Task<AddressDto?> GetDefaultAddressAsync(long userId, string type)
        {
            var address = await _addressRepository.GetDefaultAddressAsync(userId, type);
            if (address == null) return null;

            return new AddressDto
            {
                Id = address.Id,
                UserId = address.UserId,
                Street = address.Street,
                City = address.City,
                Province = address.Province,
                PostalCode = address.PostalCode,
                Country = address.Country,
                AddressType = address.AddressType,
                IsDefault = address.IsDefault
            };
        }

        public async Task AddAddressAsync(AddressDto dto)
        {
            var existing = await _addressRepository.GetUserAddressesAsync(dto.UserId);
            if (existing.Any(a =>
                a.Street == dto.Street &&
                a.City == dto.City &&
                a.Province == dto.Province &&
                a.PostalCode == dto.PostalCode &&
                a.Country == dto.Country &&
                a.AddressType == dto.AddressType))
            {
                throw new InvalidOperationException("Address already exists.");
            }

            if (dto.IsDefault)
            {
                foreach (var addr in existing.Where(a => a.AddressType == dto.AddressType))
                {
                    addr.IsDefault = false;
                }
            }

            var address = new Address
            {
                UserId = dto.UserId,
                Street = dto.Street,
                City = dto.City,
                Province = dto.Province,
                PostalCode = dto.PostalCode,
                Country = dto.Country,
                AddressType = dto.AddressType,
                IsDefault = dto.IsDefault
            };

            await _addressRepository.AddAsync(address);
            await _addressRepository.SaveChangesAsync();

            await _cache.RemoveAsync(GetKey(dto.UserId)); 
        }

        public async Task UpdateAddressAsync(AddressDto dto)
        {
            var address = await _addressRepository.GetByIdAsync(dto.Id);
            if (address == null) throw new KeyNotFoundException("Address not found.");

            if (dto.IsDefault)
            {
                var userAddresses = await _addressRepository.GetUserAddressesAsync(dto.UserId);
                foreach (var addr in userAddresses.Where(a => a.AddressType == dto.AddressType && a.Id != dto.Id))
                {
                    addr.IsDefault = false;
                }
            }

            address.Street = dto.Street;
            address.City = dto.City;
            address.Province = dto.Province;
            address.PostalCode = dto.PostalCode;
            address.Country = dto.Country;
            address.AddressType = dto.AddressType;
            address.IsDefault = dto.IsDefault;

            await _addressRepository.SaveChangesAsync();
            await _cache.RemoveAsync(GetKey(dto.UserId)); 
        }

        public async Task RemoveAddressAsync(long id)
        {
            var address = await _addressRepository.GetByIdAsync(id);
            if (address != null)
            {
                _addressRepository.Remove(address);
                await _addressRepository.SaveChangesAsync();
                await _cache.RemoveAsync(GetKey(address.UserId)); 
            }
        }
    }
}