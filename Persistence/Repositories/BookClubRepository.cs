using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class BookClubRepository(RepositoryDbContext dbContext) : IBookClubRepository
{
    public async Task<IEnumerable<BookClub>> GetAllAsync(
        CancellationToken cancellationToken = default
    ) => await dbContext.BookClubs.Include(x => x.Members).ToListAsync(cancellationToken);

    public async Task<BookClub> GetByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    ) =>
        await dbContext
            .BookClubs.Include(x => x.Members)
            .FirstAsync(x => x.Id == id, cancellationToken);

    public void Insert(BookClub owner) => dbContext.BookClubs.Add(owner);

    public void Remove(BookClub owner) => dbContext.BookClubs.Remove(owner);
}
