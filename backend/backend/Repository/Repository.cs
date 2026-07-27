using backend.Data;
using backend.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace backend.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly GymStoreContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(GymStoreContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(long id)
            => await _dbSet.FindAsync(id);

        public async Task<IEnumerable<T>> GetAllAsync()
            => await _dbSet.ToListAsync();

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
            => await _dbSet.Where(predicate).ToListAsync();

        public async Task AddAsync(T entity)
            => await _dbSet.AddAsync(entity);

        public void Remove(T entity)
            => _dbSet.Remove(entity);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}