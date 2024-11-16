using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistence.Repositories;

namespace Persistence.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDataServices(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<RepositoryDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        return services;
    }
}
