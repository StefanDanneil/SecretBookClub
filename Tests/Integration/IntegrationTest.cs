using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace Integration;

public abstract partial class IntegrationTest
{
    private readonly ApiFactory _testApi = new();
    private readonly HttpClient _client;

    protected IntegrationTest()
    {
        _client = _testApi.CreateClient();
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
        _testApi.Dispose();
    }
}
