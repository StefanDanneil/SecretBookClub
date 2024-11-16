using System.Net;
using Contracts;

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
}
