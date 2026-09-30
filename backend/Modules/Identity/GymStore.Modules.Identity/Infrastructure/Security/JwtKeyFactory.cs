using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace GymStore.Modules.Identity.Infrastructure.Security;

/// <summary>
/// Single source of truth for the JWT signing/validation key. Both token generation and token
/// validation derive the key here, so they can never diverge (the pre-refactor code signed with
/// SHA256(key) but validated with the raw key). Derives a fixed 256-bit key from the configured
/// secret so any sufficiently strong passphrase works with HS256.
/// </summary>
internal static class JwtKeyFactory
{
    public static SymmetricSecurityKey CreateSigningKey(IConfiguration configuration)
    {
        var rawKey = configuration["Jwt:Key"];
        if (string.IsNullOrWhiteSpace(rawKey))
        {
            throw new InvalidOperationException(
                "JWT signing key (Jwt:Key) is missing or empty. Configure a strong, random secret.");
        }

        var keyBytes = SHA256.HashData(Encoding.UTF8.GetBytes(rawKey));
        return new SymmetricSecurityKey(keyBytes);
    }
}
