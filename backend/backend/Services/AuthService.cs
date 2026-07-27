using backend.DTO;
using backend.Interfaces;
using backend.IRepository;
using backend.Models;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResult> RegisterAsync(RegisterDto dto)
        {
            if (await _userRepository.EmailExistsAsync(dto.Email))
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

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return new AuthResult
            {
                Success = true,
                Message = "User registered successfully."
            };
        }

        public async Task<AuthResult> LoginAsync(LoginDto dto)
        {
            var user = await _userRepository.GetByEmailAsync(dto.Email);

            if (user == null)
                return new AuthResult { Success = false, Message = "User not found." };

            if (!VerifyPassword(dto.Password, user.PasswordHash))
                return new AuthResult { Success = false, Message = "Invalid credentials." };

            user.LastLoginAt = DateTime.UtcNow;
            await _userRepository.SaveChangesAsync();

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