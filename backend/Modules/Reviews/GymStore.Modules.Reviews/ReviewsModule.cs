using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Common.Modules;
using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Reviews.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Reviews;

/// <summary>
/// Composition entry point for the Reviews module (registered via the common IModule mechanism).
/// The host supplies the <see cref="IReviewerInfoProvider"/> implementation.
/// </summary>
public sealed class ReviewsModule : IModule
{
    public Assembly Assembly => typeof(ReviewsModule).Assembly;

    public void Register(IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReviewsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddHandlersFromAssembly(Assembly);
    }
}
