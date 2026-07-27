using backend.Data;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class SupplierRepository : Repository<Supplier>, ISupplierRepository
    {
        private readonly GymStoreContext _context;

        public SupplierRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _context.Suppliers
                .Include(s => s.Inventories).ThenInclude(i => i.Product)
                .ToListAsync();
        }

        public async Task<Supplier?> GetSupplierAsync(int id)
        {
            return await _context.Suppliers
                .Include(s => s.Inventories).ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(s => s.Id == id);
        }
    }
}