using backend.Data;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class AddressRepository : Repository<Address>, IAddressRepository
    {
        private readonly GymStoreContext _context;

        public AddressRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Address>> GetUserAddressesAsync(long userId)
        {
            return await _context.Addresses
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<Address?> GetDefaultAddressAsync(long userId, string type)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.UserId == userId && a.AddressType == type && a.IsDefault);
        }
    }
}