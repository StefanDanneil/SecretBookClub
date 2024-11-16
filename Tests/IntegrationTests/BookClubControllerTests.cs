using System.Net;
using Contracts;

namespace IntegrationTests;

[TestClass]
public class BookClubControllerTests : IntegrationTest
{
    [TestMethod]
    public async Task it_is_possible_to_create_a_new_book_club()
    {
        var response = await HttpPost<BookClubDto>(
            "BookClub",
            new BookClubForCreationDto("My Book Club")
        );

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(response.Content?.Id);
        Assert.AreNotEqual(0, response.Content.Id);
        Assert.AreEqual("My Book Club", response.Content.Name);
    }
}
