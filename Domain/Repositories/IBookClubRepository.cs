using Domain.Entities;

namespace Domain.Repositories;

public interface IBookClubRepository
{
    Task<IEnumerable<BookClub>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookClub> GetByIdAsync(int ownerId, CancellationToken cancellationToken = default);
    void Insert(BookClub bookClub);
    void Remove(BookClub bookClub);
}