using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly GymStoreContext _context;
        public SupplierService(GymStoreContext context) => _context = context;

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