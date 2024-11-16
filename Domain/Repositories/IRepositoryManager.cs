namespace Domain.Repositories;

public interface IRepositoryManager
{
    public IUserRepository UserRepository { get; }
    public IBookClubRepository BookClubRepository { get; }
    public IUnitOfWork UnitOfWork { get; }
}
