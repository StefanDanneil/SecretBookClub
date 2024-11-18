using System.Net;
using Contracts;
using Domain.Entities;

namespace Integration.Tests;

[TestClass]
public class BookClubControllerTests : IntegrationTest
{
    [TestMethod]
    public async Task it_is_possible_to_create_a_new_book_club()
    {
        var response = await PostAsync<BookClubDto>(
            "BookClub",
            new BookClubForCreationDto("My Book Club")
        );

        Assert.AreEqual(HttpStatusCode.Created, response.StatusCode);
        Assert.IsNotNull(response.Content?.Id);
        Assert.AreNotEqual(0, response.Content.Id);
        Assert.AreEqual("My Book Club", response.Content.Name);
    }

    [TestMethod]
    public async Task it_is_possible_to_get_all_book_clubs()
    {
        var firstBookClub = await CreateEntity(new BookClub { Name = "My First Club" });
        var secondBookClub = await CreateEntity(new BookClub { Name = "My Second Club" });

        var response = await GetAsync<IEnumerable<BookClubDto>>("BookClub");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(2, response.Content?.Count());

        var firstApiClub = response.Content!.First(b => b.Name == firstBookClub.Name);
        var secondApiClub = response.Content!.First(b => b.Name == secondBookClub.Name);

        Assert.AreNotEqual(0, firstApiClub.Id);
        Assert.AreNotEqual(0, secondApiClub.Id);
        Assert.IsNotNull(firstApiClub.Id);
        Assert.IsNotNull(secondApiClub.Id);
        Assert.AreEqual(firstBookClub.Name, firstApiClub.Name);
        Assert.AreEqual(secondBookClub.Name, secondApiClub.Name);
    }

    [TestMethod]
    public async Task it_is_possible_to_get_a_specific_book_club_by_id()
    {
        await CreateEntity(new BookClub { Name = "My First Club" });
        var secondBookClub = await CreateEntity(new BookClub { Name = "My Second Club" });

        var response = await GetAsync<BookClubDto>($"BookClub/{secondBookClub.Id}");

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(secondBookClub.Id, response.Content?.Id);
        Assert.AreEqual(secondBookClub.Name, response.Content?.Name);
    }

    [TestMethod]
    public async Task it_is_possible_to_update_a_book_club()
    {
        var originalBookClub = await CreateEntity(new BookClub { Name = "My First Club" });

        var response = await PutAsync<BookClubDto>(
            $"BookClub/{originalBookClub.Id}",
            new BookClubForUpdateDto("My new name")
        );

        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual(originalBookClub.Id, response.Content?.Id);
        Assert.AreEqual("My new name", response.Content?.Name);
    }

    [TestMethod]
    public async Task it_is_possible_to_delete_a_book_club()
    {
        var bookClubToDelete = await CreateEntity(new BookClub { Name = "My First Club" });

        var firstGetResponse = await GetAsync($"BookClub/{bookClubToDelete.Id}");
        var deleteResponse = await DeleteAsync($"BookClub/{bookClubToDelete.Id}");
        var secondGetResponse = await GetAsync($"BookClub/{bookClubToDelete.Id}");

        Assert.AreEqual(HttpStatusCode.OK, firstGetResponse.StatusCode);
        Assert.AreEqual(HttpStatusCode.NoContent, deleteResponse.StatusCode);
        Assert.AreEqual(HttpStatusCode.NotFound, secondGetResponse.StatusCode);
    }
}
