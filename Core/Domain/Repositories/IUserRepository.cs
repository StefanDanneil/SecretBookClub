using Domain.Entities;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllByBookClubIdAsync(int bookClubId, CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(int accountId, CancellationToken cancellationToken = default);
    void Insert(User user);
    void Remove(User user);    
}
