using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Persistence;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace Integration;

public abstract class IntegrationTest
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

    protected async Task<DeserializedHttpResponse<T>> HttpGet<T>(string path)
    {
        var response = await _client.GetAsync(path);
        return await GetDeserializedResponse<T>(response);
    }

    protected async Task<DeserializedHttpResponse<T>> HttpDelete<T>(string path)
    {
        var response = await _client.DeleteAsync(path);
        return await GetDeserializedResponse<T>(response);
    }

    protected async Task<DeserializedHttpResponse<T>> HttpPost<T>(string path, object payload)
    {
        var response = await _client.PostAsync(path, PreparePayload(payload));
        return await GetDeserializedResponse<T>(response);
    }

    protected async Task<DeserializedHttpResponse<T>> HttpPut<T>(string path, object payload)
    {
        var response = await _client.PutAsync(path, PreparePayload(payload));
        return await GetDeserializedResponse<T>(response);
    }

    private static StringContent? PreparePayload(object? payload)
    {
        if (payload is null)
            return null;

        var json = JsonConvert.SerializeObject(payload);
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    protected class DeserializedHttpResponse<T>
    {
        public HttpStatusCode StatusCode { get; init; }
        public T? Content { get; set; }
    }

    private async Task<DeserializedHttpResponse<T>> GetDeserializedResponse<T>(
        HttpResponseMessage response
    )
    {
        var content = await response.Content.ReadAsStringAsync();

        var result = new DeserializedHttpResponse<T> { StatusCode = response.StatusCode };
        if (content == string.Empty)
            return result;

        result.Content =
            typeof(T) == typeof(string)
                ? (T)(object)content
                : JsonSerializer.Deserialize<T>(content, _serializerOptions);

        return result;
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
