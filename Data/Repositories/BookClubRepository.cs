using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

internal class BookClubRepository(AppDbContext dbContext) : IBookClubRepository
{
    public async Task<BookClub> GetByIdAsync(int id)
    {
        return await dbContext.BookClubs.SingleAsync(bc => bc.Id == id);
    }
}