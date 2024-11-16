using System.Net;
using Contracts;
using Domain.Entities;

namespace Integration.Tests;

[TestClass]
public class UserControllerTests : IntegrationTest
{
    [TestMethod]
    public async Task it_is_possible_to_create_a_new_user()
    {
        var response = await HttpPost<UserDto>("User", new UserForCreationDto("My First User"));

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(response.Content?.Id);
        Assert.AreNotEqual(0, response.Content.Id);
        Assert.AreEqual("My First User", response.Content.Name);
    }

    [TestMethod]
    public async Task it_is_possible_to_get_a_specific_user_by_id()
    {
        await CreateEntity(new User { Name = "My First User" });
        var secondUser = await CreateEntity(new User { Name = "My Second User" });

        var response = await HttpGet<UserDto>($"User/{secondUser.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(secondUser.Id, response.Content?.Id);
        Assert.AreEqual(secondUser.Name, response.Content?.Name);
    }

    [TestMethod]
    public async Task it_is_possible_to_delete_a_user()
    {
        var userToDelete = await CreateEntity(new User { Name = "My First User" });

        var firstGetResponse = await HttpGet<UserDto>($"User/{userToDelete.Id}");
        var deleteResponse = await HttpDelete<string>($"User/{userToDelete.Id}");
        var secondGetResponse = await HttpGet<string>($"User/{userToDelete.Id}");

        Assert.AreEqual(HttpStatusCode.OK, firstGetResponse.StatusCode);
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, secondGetResponse.StatusCode);

        Assert.IsNull(deleteResponse.Content);
    }
}
