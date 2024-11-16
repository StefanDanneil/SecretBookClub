using Domain.Repositories;

namespace Persistence.Repositories;

public sealed class RepositoryManager(RepositoryDbContext dbContext) : IRepositoryManager
{
    private readonly Lazy<IUserRepository> _lazyUserRepository =
        new(() => new UserRepository(dbContext));
    private readonly Lazy<IBookClubRepository> _lazyBookClubRepository =
        new(() => new BookClubRepository(dbContext));
    private readonly Lazy<IUnitOfWork> _lazyUnitOfWork = new(() => new UnitOfWork(dbContext));

    public IUserRepository UserRepository => _lazyUserRepository.Value;
    public IBookClubRepository BookClubRepository => _lazyBookClubRepository.Value;
    public IUnitOfWork UnitOfWork => _lazyUnitOfWork.Value;
}
