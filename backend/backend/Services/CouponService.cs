using backend.Data;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class CouponService : ICouponService
    {
        private readonly GymStoreContext _context;
        public CouponService(GymStoreContext context) => _context = context;

        public async Task<Coupon?> ValidateCouponAsync(string code, decimal orderAmount)
        {
            var coupon = await _context.Coupons.FirstOrDefaultAsync(c => c.Code == code);

            if (coupon == null || coupon.EndDate < DateTime.UtcNow)
                return null;

            if (orderAmount < coupon.DiscountValue)
                return null;

            return coupon;
        }
    }
}