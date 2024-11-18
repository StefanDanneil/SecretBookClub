using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Integration;

public abstract partial class IntegrationTest
{
    private readonly ApiFactory _testApi = new();
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _serializerOptions =
        new() { PropertyNameCaseInsensitive = true };

    protected IntegrationTest()
    {
        _client = _testApi.CreateClient();
    }

    private RepositoryDbContext CreateDbContext()
    {
        var scope = _testApi.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();
        return context;
    }

    // protected async Task<T?> FindEntityById<T>(int id)
    //     where T : class, IEntity
    // {
    //     await using var context = CreateDbContext();
    //     return await context.Set<T>().FindAsync(id);
    // }

    protected async Task<TEntity> CreateEntity<TEntity>(TEntity entity)
        where TEntity : class
    {
        await using var context = CreateDbContext();
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    [TestInitialize]
    public async Task BaseSetup()
    {
        await using var context = CreateDbContext();
        await context.Database.MigrateAsync();
    }

    [TestCleanup]
    public async Task BaseTeardown()
    {
        await using var context = CreateDbContext();
        await context.Database.EnsureDeletedAsync();
    }

    [AssemblyCleanup]
    public void BaseOneTimeTeardown()
    {
        _client.Dispose();
    }
}
