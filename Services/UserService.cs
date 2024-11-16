using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;

namespace Services;

internal sealed class UserService(IRepositoryManager repositoryManager) : IUserService
{
    public async Task<IEnumerable<UserDto>> GetAllByBookClubIdAsync(
        int bookClubId,
        CancellationToken cancellationToken = default
    )
    {
        var users = await repositoryManager.UserRepository.GetAllByBookClubIdAsync(
            bookClubId,
            cancellationToken
        );
        var usersDto = users.Adapt<IEnumerable<UserDto>>();
        return usersDto;
    }

    public async Task<UserDto> GetByIdAsync(int userId, CancellationToken cancellationToken)
    {
        var user = await repositoryManager.UserRepository.GetByIdAsync(userId, cancellationToken);
        if (user is null)
        {
            throw new UserNotFoundException(userId);
        }
        var userDto = user.Adapt<UserDto>();
        return userDto;
    }

    public async Task<UserDto> CreateAsync(
        int bookClubId,
        UserForCreationDto userForCreationDto,
        CancellationToken cancellationToken = default
    )
    {
        var bookClub = await repositoryManager.BookClubRepository.GetByIdAsync(
            bookClubId,
            cancellationToken
        );
        if (bookClub is null)
        {
            throw new BookClubNotFoundException(bookClubId);
        }
        var user = userForCreationDto.Adapt<User>();
        repositoryManager.UserRepository.Insert(user);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        return user.Adapt<UserDto>();
    }

    public async Task DeleteAsync(int bookClubId, CancellationToken cancellationToken = default)
    {
        var bookClub = await repositoryManager.BookClubRepository.GetByIdAsync(
            bookClubId,
            cancellationToken
        );
        if (bookClub is null)
        {
            throw new BookClubNotFoundException(bookClubId);
        }
        repositoryManager.BookClubRepository.Remove(bookClub);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
    }
}
