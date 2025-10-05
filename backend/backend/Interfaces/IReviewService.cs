using backend.Models;

namespace backend.Interfaces
{
    public interface IReviewService
    {
        Task AddReviewAsync(int userId, int productId, string content, int rating);
        Task<IEnumerable<Review>> GetProductReviewsAsync(int productId);
    }
}