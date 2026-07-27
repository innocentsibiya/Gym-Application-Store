using backend.Data;
using backend.IRepository;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Repository
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly GymStoreContext _context;

        public ReviewRepository(GymStoreContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(int userId, int productId, string content, int rating)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            var review = new Review
            {
                UserId = userId,
                ProductId = productId,
                Comment = content,
                Rating = rating,
                CreatedAt = DateTime.UtcNow
            };

            _context.Reviews.Add(review);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
        {
            return await _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .ToListAsync();
        }
    }
}