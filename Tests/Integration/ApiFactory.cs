using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persistence;

namespace Integration;

internal class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder
            .ConfigureLogFilters()
            .ConfigureTestServices(services =>
            {
                services.ConfigureTestDb(
                    "Host=localhost;Username=docker;Password=secret;Database=testdatabase;Port=5434"
                );
            });
    }
}

public static class Extensions
{
    public static IServiceCollection ConfigureTestDb(
        this IServiceCollection services,
        string connectionString
    )
    {
        var descriptor = services.SingleOrDefault(d =>
            d.ServiceType == typeof(DbContextOptions<RepositoryDbContext>)
        );

        if (descriptor != null)
        {
            services.Remove(descriptor);
        }

        services.AddDbContext<RepositoryDbContext>(options => options.UseNpgsql(connectionString));

        return services;
    }

    public static IWebHostBuilder ConfigureLogFilters(this IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging =>
        {
            logging.AddFilter("Web.Middlewares.ExceptionHandlingMiddleware", LogLevel.None);
            logging.AddFilter(
                "Microsoft.AspNetCore.HttpsPolicy.HttpsRedirectionMiddleware",
                LogLevel.None
            );
            logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.None);
            logging.AddFilter("Microsoft.EntityFrameworkCore.Migrations", LogLevel.None);
        });

        return builder;
    }
}
