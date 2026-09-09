using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Identity.Application.Abstractions;
using GymStore.Modules.Identity.Contracts;
using GymStore.Modules.Identity.Infrastructure.Persistence;
using GymStore.Modules.Identity.Infrastructure.Public;
using GymStore.Modules.Identity.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace GymStore.Modules.Identity;

/// <summary>
/// Composition entry point for the Identity module. Besides its own services it configures JWT
/// bearer authentication, deriving the validation key from the same <see cref="JwtKeyFactory"/>
/// the token service signs with — so signing and validation cannot diverge. The host still owns
/// the pipeline (app.UseAuthentication()).
/// </summary>
public sealed class IdentityModule : IModule
{
    public Assembly Assembly => typeof(IdentityModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IdentityDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IUserRepository, UserRepository>();
        services.AddSingleton<IPasswordHasher, Pbkdf2PasswordHasher>();
        services.AddScoped<ITokenService, JwtTokenService>();
        services.AddScoped<IIdentityModuleApi, IdentityModuleApi>();
        services.AddHandlersFromAssembly(Assembly);

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["Jwt:Issuer"],
                ValidAudience = configuration["Jwt:Audience"],
                IssuerSigningKey = JwtKeyFactory.CreateSigningKey(configuration)
            };
        });
    }
}
