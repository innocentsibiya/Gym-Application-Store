using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class SupplierService : ISupplierService
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierService(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }

        public async Task<IEnumerable<Supplier>> GetAllSuppliersAsync()
        {
            return await _supplierRepository.GetAllSuppliersAsync();
        }

        public async Task<Supplier?> GetSupplierAsync(int id)
        {
            return await _supplierRepository.GetSupplierAsync(id);
        }
    }
}