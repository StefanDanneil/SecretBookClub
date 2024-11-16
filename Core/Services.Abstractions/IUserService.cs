using Contracts;

namespace Services.Abstractions;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllByBookClubIdAsync(
        int bookClubId,
        CancellationToken cancellationToken = default
    );
    Task<UserDto> GetByIdAsync(int userId, CancellationToken cancellationToken);
    Task<UserDto> CreateAsync(
        UserForCreationDto userForCreationDto,
        CancellationToken cancellationToken = default
    );
    Task DeleteAsync(int userId, CancellationToken cancellationToken = default);
}
