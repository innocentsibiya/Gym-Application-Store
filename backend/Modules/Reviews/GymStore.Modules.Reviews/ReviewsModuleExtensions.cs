using System.Reflection;
using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Reviews.Application.Abstractions;
using GymStore.Modules.Reviews.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GymStore.Modules.Reviews;

/// <summary>
/// Composition entry point for the Reviews module. The host calls <see cref="AddReviewsModule"/>,
/// registers <see cref="Assembly"/> as an MVC application part, and supplies the
/// <see cref="IReviewerInfoProvider"/> implementation.
/// </summary>
public static class ReviewsModuleExtensions
{
    /// <summary>The module assembly (used by the host for controller discovery).</summary>
    public static Assembly Assembly => typeof(ReviewsModuleExtensions).Assembly;

    public static IServiceCollection AddReviewsModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ReviewsDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

        services.AddScoped<IReviewRepository, ReviewRepository>();
        services.AddHandlersFromAssembly(Assembly);

        return services;
    }
}
