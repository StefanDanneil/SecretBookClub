using Domain.Repositories;
using Services.Abstractions;

namespace Services;

public sealed class ServiceManager(IRepositoryManager repositoryManager) : IServiceManager
{
    private readonly Lazy<IUserService> _lazyUserService =
        new(() => new UserService(repositoryManager));
    private readonly Lazy<IBookClubService> _lazyBookClubService =
        new(() => new BookClubService(repositoryManager));

    public IUserService UserService => _lazyUserService.Value;
    public IBookClubService BookClubService => _lazyBookClubService.Value;
}
