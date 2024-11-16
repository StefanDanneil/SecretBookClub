using Microsoft.Extensions.DependencyInjection;
using Services.Abstractions;

namespace Services.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services.AddScoped<IServiceManager, ServiceManager>();

        return services;
    }
}
