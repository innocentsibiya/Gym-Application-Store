using backend.Data;
using backend.DTO;
using backend.Interfaces;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly GymStoreContext _context;
        private readonly ITokenService _tokenService;

        public AuthService(GymStoreContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
                return new AuthResult { Success = false, Message = "Email already in use." };

            var passwordHash = HashPassword(dto.Password);

            var user = new User
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                PasswordHash = passwordHash,
                Role = "Customer",
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Message = "User registered successfully."
            };
        }

        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null)
                return new AuthResult { Success = false, Message = "User not found." };

            if (!VerifyPassword(dto.Password, user.PasswordHash))
                return new AuthResult { Success = false, Message = "Invalid credentials." };

            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var token = _tokenService.GenerateToken(user);

            return new AuthResult
            {
                Success = true,
                Message = "Login successful.",
                Token = token,
                User = user
            };
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(bytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }
}