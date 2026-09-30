using System.Security.Cryptography;
using System.Text;
using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Application.Contracts;
using GymStore.Modules.Identity.Application.Features.Login;
using GymStore.Modules.Identity.Application.Features.Register;
using GymStore.Modules.Identity.Domain;
using GymStore.Modules.Identity.Infrastructure.Persistence;
using GymStore.Modules.Identity.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Identity.Tests;

public class AuthFlowTests
{
    private static IdentityDbContext NewContext() =>
        new(new DbContextOptionsBuilder<IdentityDbContext>()
            .UseInMemoryDatabase($"identity-tests-{Guid.NewGuid()}")
            .Options);

    private static RegisterRequest NewRegister(string email = "new@gymstore.test") => new()
    {
        FirstName = "Ada", LastName = "Lovelace", Email = email, Password = "Password123"
    };

    private sealed class FakeTokenService : ITokenService
    {
        public string GenerateToken(User user) => $"token-for-{user.Id}";
    }

    [Fact]
    public async Task Register_NewEmail_PersistsPbkdf2Hash()
    {
        await using var ctx = NewContext();
        var repo = new UserRepository(ctx);
        var hasher = new Pbkdf2PasswordHasher();
        var handler = new RegisterCommandHandler(repo, hasher);

        var result = await handler.Handle(new RegisterCommand(NewRegister()), CancellationToken.None);

        Assert.True(result.Success);
        var user = await ctx.Users.SingleAsync();
        Assert.NotEqual("Password123", user.PasswordHash);                 // not plaintext
        Assert.Equal(PasswordVerification.Success, hasher.Verify(user.PasswordHash, "Password123"));
    }

    [Fact]
    public async Task Register_ExistingEmail_Fails()
    {
        await using var ctx = NewContext();
        var repo = new UserRepository(ctx);
        var hasher = new Pbkdf2PasswordHasher();
        var handler = new RegisterCommandHandler(repo, hasher);
        await handler.Handle(new RegisterCommand(NewRegister()), CancellationToken.None);

        var again = await handler.Handle(new RegisterCommand(NewRegister()), CancellationToken.None);

        Assert.False(again.Success);
        Assert.Equal("Email already in use.", again.Message);
    }

    [Fact]
    public async Task Login_CorrectPassword_ReturnsTokenAndUser()
    {
        await using var ctx = NewContext();
        var repo = new UserRepository(ctx);
        var hasher = new Pbkdf2PasswordHasher();
        await new RegisterCommandHandler(repo, hasher).Handle(new RegisterCommand(NewRegister()), CancellationToken.None);

        var handler = new LoginCommandHandler(repo, hasher, new FakeTokenService());
        var result = await handler.Handle(
            new LoginCommand(new LoginRequest { Email = "new@gymstore.test", Password = "Password123" }),
            CancellationToken.None);

        Assert.True(result.Success);
        Assert.False(string.IsNullOrEmpty(result.Token));
        Assert.NotNull(result.User);
        Assert.Equal("new@gymstore.test", result.User!.Email);
    }

    [Theory]
    [InlineData("new@gymstore.test", "wrongpassword")] // right user, wrong password
    [InlineData("nobody@gymstore.test", "Password123")] // unknown user
    public async Task Login_BadCredentials_ReturnGenericFailure(string email, string password)
    {
        await using var ctx = NewContext();
        var repo = new UserRepository(ctx);
        var hasher = new Pbkdf2PasswordHasher();
        await new RegisterCommandHandler(repo, hasher).Handle(new RegisterCommand(NewRegister()), CancellationToken.None);

        var handler = new LoginCommandHandler(repo, hasher, new FakeTokenService());
        var result = await handler.Handle(
            new LoginCommand(new LoginRequest { Email = email, Password = password }), CancellationToken.None);

        Assert.False(result.Success);
        Assert.Equal("Invalid credentials.", result.Message); // identical → no enumeration
        Assert.Null(result.Token);
    }

    [Fact]
    public async Task Login_LegacyHashUser_UpgradesHashOnSuccess()
    {
        await using var ctx = NewContext();
        var legacyHash = Convert.ToBase64String(SHA256.HashData(Encoding.UTF8.GetBytes("Password123")));
        ctx.Users.Add(new User { Email = "legacy@gymstore.test", FirstName = "Old", LastName = "User", PasswordHash = legacyHash });
        await ctx.SaveChangesAsync();

        var repo = new UserRepository(ctx);
        var hasher = new Pbkdf2PasswordHasher();
        var handler = new LoginCommandHandler(repo, hasher, new FakeTokenService());

        var result = await handler.Handle(
            new LoginCommand(new LoginRequest { Email = "legacy@gymstore.test", Password = "Password123" }),
            CancellationToken.None);

        Assert.True(result.Success);
        var user = await ctx.Users.SingleAsync(u => u.Email == "legacy@gymstore.test");
        Assert.NotEqual(legacyHash, user.PasswordHash); // upgraded away from legacy SHA-256
        Assert.Equal(PasswordVerification.Success, hasher.Verify(user.PasswordHash, "Password123"));
    }
}
