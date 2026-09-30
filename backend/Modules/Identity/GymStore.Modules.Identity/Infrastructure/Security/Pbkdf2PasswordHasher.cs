using System.Buffers.Binary;
using System.Security.Cryptography;
using System.Text;
using GymStore.Modules.Identity.Application.Abstractions;

namespace GymStore.Modules.Identity.Infrastructure.Security;

/// <summary>
/// PBKDF2 (HMAC-SHA256) password hasher with a per-password random salt and constant-time
/// comparison. Understands the legacy unsalted SHA-256 format for a transparent, one-time
/// upgrade: legacy matches verify successfully but report <see cref="PasswordVerification.SuccessRehashNeeded"/>.
/// </summary>
internal sealed class Pbkdf2PasswordHasher : IPasswordHasher
{
    private const byte FormatVersion = 1;
    private const int SaltSize = 16;      // 128-bit salt
    private const int SubkeySize = 32;    // 256-bit derived key
    private const int Iterations = 100_000;
    private const int HeaderSize = 1 + sizeof(int); // version + iteration count
    private static readonly HashAlgorithmName Prf = HashAlgorithmName.SHA256;

    public string Hash(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltSize);
        var subkey = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Prf, SubkeySize);

        var blob = new byte[HeaderSize + SaltSize + SubkeySize];
        blob[0] = FormatVersion;
        BinaryPrimitives.WriteInt32BigEndian(blob.AsSpan(1, sizeof(int)), Iterations);
        salt.CopyTo(blob.AsSpan(HeaderSize, SaltSize));
        subkey.CopyTo(blob.AsSpan(HeaderSize + SaltSize, SubkeySize));

        return Convert.ToBase64String(blob);
    }

    public PasswordVerification Verify(string storedHash, string password)
    {
        byte[] decoded;
        try
        {
            decoded = Convert.FromBase64String(storedHash);
        }
        catch (FormatException)
        {
            return PasswordVerification.Failed;
        }

        // Legacy unsalted SHA-256: the stored value is exactly the 32-byte digest, base64-encoded.
        if (decoded.Length == SubkeySize)
        {
            var legacy = SHA256.HashData(Encoding.UTF8.GetBytes(password));
            return CryptographicOperations.FixedTimeEquals(legacy, decoded)
                ? PasswordVerification.SuccessRehashNeeded
                : PasswordVerification.Failed;
        }

        // Current PBKDF2 packed format.
        if (decoded.Length != HeaderSize + SaltSize + SubkeySize || decoded[0] != FormatVersion)
        {
            return PasswordVerification.Failed;
        }

        var iterations = BinaryPrimitives.ReadInt32BigEndian(decoded.AsSpan(1, sizeof(int)));
        var salt = decoded.AsSpan(HeaderSize, SaltSize).ToArray();
        var expected = decoded.AsSpan(HeaderSize + SaltSize, SubkeySize).ToArray();

        var actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, Prf, SubkeySize);
        return CryptographicOperations.FixedTimeEquals(actual, expected)
            ? PasswordVerification.Success
            : PasswordVerification.Failed;
    }
}
