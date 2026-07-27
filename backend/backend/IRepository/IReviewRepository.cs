using backend.Models;

namespace backend.IRepository
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task AddReviewAsync(int userId, int productId, string content, int rating);
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
    }
}