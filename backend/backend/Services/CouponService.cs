using backend.Interfaces;
using backend.IRepository;
using backend.Models;

namespace backend.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<Coupon?> ValidateCouponAsync(string code, decimal orderAmount)
        {
            return await _couponRepository.ValidateCouponAsync(code, orderAmount);
        }
    }
}