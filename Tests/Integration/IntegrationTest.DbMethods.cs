using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;
using Persistence;

namespace Integration;

public abstract partial class IntegrationTest
{
    private RepositoryDbContext CreateDbContext()
    {
        var scope = _testApi.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<RepositoryDbContext>();
        return context;
    }

    protected async Task<T?> FindEntityByIdAsync<T>(int id)
        where T : class, IEntity
    {
        await using var context = CreateDbContext();
        return await context.Set<T>().FindAsync(id);
    }

    protected async Task<TEntity> CreateEntityAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        await using var context = CreateDbContext();
        context.Set<TEntity>().Add(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
