using GymStore.Modules.Identity.Domain;

namespace GymStore.Modules.Identity.Application.Abstractions;

/// <summary>Issues signed access tokens for authenticated users.</summary>
public interface ITokenService
{
    string GenerateToken(User user);
}
