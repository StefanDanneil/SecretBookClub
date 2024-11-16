using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace IntegrationTests;

internal class ApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Remove the app's ApplicationDbContext registration.
            var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<RepositoryDbContext>)
            );

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<RepositoryDbContext>(options =>
                options.UseNpgsql(
                    "Host=localhost;Username=docker;Password=secret;Database=testdatabase;Port=5434"
                )
            );
        });
    }
}
