using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Application.Contracts;

namespace GymStore.Modules.Identity.Application.Features.Login;

/// <summary>
/// Authenticates a user. Returns a single generic failure for both an unknown email and a wrong
/// password (anti-enumeration), and spends comparable CPU time when the user is absent so the
/// response timing doesn't reveal account existence. Legacy password hashes are upgraded on
/// successful login.
/// </summary>
internal sealed class LoginCommandHandler : ICommandHandler<LoginCommand, AuthResult>
{
    private const string GenericFailure = "Invalid credentials.";

    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(IUserRepository users, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<AuthResult> Handle(LoginCommand command, CancellationToken ct)
    {
        var dto = command.Request;
        var user = await _users.GetByEmailAsync(dto.Email, ct);

        if (user is null)
        {
            // Spend a comparable PBKDF2 cost so a missing account isn't faster than a wrong password.
            _ = _passwordHasher.Hash(dto.Password);
            return AuthResult.Fail(GenericFailure);
        }

        var verification = _passwordHasher.Verify(user.PasswordHash, dto.Password);
        if (verification == PasswordVerification.Failed)
        {
            return AuthResult.Fail(GenericFailure);
        }

        if (verification == PasswordVerification.SuccessRehashNeeded)
        {
            user.PasswordHash = _passwordHasher.Hash(dto.Password); // transparent upgrade
        }

        user.LastLoginAt = DateTime.UtcNow;
        await _users.SaveChangesAsync(ct);

        var token = _tokenService.GenerateToken(user);

        return new AuthResult
        {
            Success = true,
            Message = "Login successful.",
            Token = token,
            User = new UserInfo(user.Id, user.FirstName, user.LastName, user.Email, user.Role)
        };
    }
}
