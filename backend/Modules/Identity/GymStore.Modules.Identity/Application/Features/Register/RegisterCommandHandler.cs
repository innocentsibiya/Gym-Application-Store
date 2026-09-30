using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Application.Contracts;
using GymStore.Modules.Identity.Domain;

namespace GymStore.Modules.Identity.Application.Features.Register;

internal sealed class RegisterCommandHandler : ICommandHandler<RegisterCommand, AuthResult>
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository users, IPasswordHasher passwordHasher)
    {
        _users = users;
        _passwordHasher = passwordHasher;
    }

    public async Task<AuthResult> Handle(RegisterCommand command, CancellationToken ct)
    {
        var dto = command.Request;

        if (await _users.EmailExistsAsync(dto.Email, ct))
        {
            return AuthResult.Fail("Email already in use.");
        }

        var user = new User
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            PasswordHash = _passwordHasher.Hash(dto.Password),
            Role = "Customer",
            CreatedAt = DateTime.UtcNow
        };

        await _users.AddAsync(user, ct);
        await _users.SaveChangesAsync(ct);

        return new AuthResult { Success = true, Message = "User registered successfully." };
    }
}
