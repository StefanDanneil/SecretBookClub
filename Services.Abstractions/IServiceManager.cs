namespace Services.Abstractions;

public interface IServiceManager
{
    IUserService UserService { get; }
    IBookClubService BookClubService { get; }
}