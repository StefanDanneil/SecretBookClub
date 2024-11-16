using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

internal sealed class UserRepository(RepositoryDbContext dbContext) : IUserRepository
{
    public async Task<IEnumerable<User>> GetAllByBookClubIdAsync(
        int bookClubId,
        CancellationToken cancellationToken = default
    ) =>
        await dbContext
            .Users.Where(x => x.BookClubs.Any(b => b.Id == bookClubId))
            .ToListAsync(cancellationToken);

    public async Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        await dbContext.Users.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void Insert(User account) => dbContext.Users.Add(account);

    public void Remove(User account) => dbContext.Users.Remove(account);
}
