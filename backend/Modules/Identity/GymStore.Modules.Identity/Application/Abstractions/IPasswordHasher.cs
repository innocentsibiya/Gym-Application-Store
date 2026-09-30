namespace GymStore.Modules.Identity.Application.Abstractions;

/// <summary>Outcome of verifying a password against a stored hash.</summary>
public enum PasswordVerification
{
    /// <summary>The password does not match.</summary>
    Failed,

    /// <summary>The password matches and the stored hash is current.</summary>
    Success,

    /// <summary>The password matches but the stored hash is a legacy/weaker format and should
    /// be re-hashed with the current algorithm and persisted.</summary>
    SuccessRehashNeeded
}

/// <summary>Hashes and verifies passwords. Implementations must be salted and slow (a KDF).</summary>
public interface IPasswordHasher
{
    string Hash(string password);
    PasswordVerification Verify(string storedHash, string password);
}
