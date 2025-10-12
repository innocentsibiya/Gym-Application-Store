using backend.Interfaces;
using backend.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backend.Services.Auth
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _config;

        public TokenService(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var rawKey = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(rawKey))
            {
                throw new InvalidOperationException("JWT signing key is missing or empty in the configuration.");
            }

            var keyBytes = Encoding.UTF8.GetBytes(rawKey);

            using (var sha256 = SHA256.Create())
            {
                var hashedKey = sha256.ComputeHash(keyBytes);
                var symmetricKey = new SymmetricSecurityKey(hashedKey); 

                var creds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.UtcNow.AddHours(2),
                    signingCredentials: creds
                );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}