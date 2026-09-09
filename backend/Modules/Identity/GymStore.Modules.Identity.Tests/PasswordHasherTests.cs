using System.Security.Cryptography;
using System.Text;
using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Infrastructure.Security;

namespace GymStore.Modules.Identity.Tests;

public class PasswordHasherTests
{
    private readonly Pbkdf2PasswordHasher _hasher = new();

    [Fact]
    public void Hash_Then_Verify_Succeeds()
    {
        var hash = _hasher.Hash("Password123");
        Assert.Equal(PasswordVerification.Success, _hasher.Verify(hash, "Password123"));
    }

    [Fact]
    public void Verify_WrongPassword_Fails()
    {
        var hash = _hasher.Hash("Password123");
        Assert.Equal(PasswordVerification.Failed, _hasher.Verify(hash, "wrong"));
    }

    [Fact]
    public void Hash_IsSalted_SoTwoHashesDiffer()
    {
        Assert.NotEqual(_hasher.Hash("Password123"), _hasher.Hash("Password123"));
    }

    [Fact]
    public void Hash_IsNotPlaintextOrRawSha256()
    {
        var hash = _hasher.Hash("Password123");
        Assert.DoesNotContain("Password123", hash);
        var sha = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("Password123")));
        Assert.NotEqual(sha, hash);
    }

    [Fact]
    public void Verify_LegacySha256_Succeeds_ButNeedsRehash()
    {
        // The pre-refactor format: base64 of the raw SHA-256 digest.
        var legacy = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("Password123")));

        Assert.Equal(PasswordVerification.SuccessRehashNeeded, _hasher.Verify(legacy, "Password123"));
        Assert.Equal(PasswordVerification.Failed, _hasher.Verify(legacy, "wrong"));
    }
}
