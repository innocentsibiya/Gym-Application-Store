using backend.Models;

namespace backend.IRepository
{
    public interface ICouponRepository : IRepository<Coupon>
    {
        Task<Coupon?> ValidateCouponAsync(string code, decimal orderAmount);
    }
}