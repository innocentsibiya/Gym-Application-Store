using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewService(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task AddReviewAsync(int userId, int productId, string content, int rating)
        {
            await _reviewRepository.AddReviewAsync(userId, productId, content, rating);
        }

        public async Task<IEnumerable<Review>> GetProductReviewsAsync(int productId)
        {
            return await _reviewRepository.GetProductReviewsAsync(productId);
        }
    }
}