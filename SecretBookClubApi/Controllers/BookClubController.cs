using Data.Entities;
using Data.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace SecretBookClub.Controllers;

[ApiController]
[Route("[controller]")]
public class BookClubController(IBookClubRepository repository) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<BookClub> Get(int id)
    {
        return await repository.GetByIdAsync(id);
    }
}