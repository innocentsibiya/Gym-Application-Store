using backend.Models;

namespace backend.Interfaces
{
    public interface ICouponService
    {
        Task<Coupon?> ValidateCouponAsync(string code, decimal orderAmount);
    }
}