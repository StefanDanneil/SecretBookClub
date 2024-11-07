using Data.Entities;

namespace Data.Interfaces;

public interface IBookClubRepository
{
    public Task<BookClub> GetByIdAsync(int id);
}