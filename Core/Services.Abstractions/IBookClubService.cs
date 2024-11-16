using Contracts;

namespace Services.Abstractions;

public interface IBookClubService
{
    Task<IEnumerable<BookClubDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<BookClubDto> GetByIdAsync(int bookClubId, CancellationToken cancellationToken = default);
    Task<BookClubDto> CreateAsync(
        BookClubForCreationDto bookClubForCreationDto,
        CancellationToken cancellationToken = default
    );
    Task<BookClubDto> UpdateAsync(
        int bookClubId,
        BookClubForUpdateDto bookClubForUpdateDto,
        CancellationToken cancellationToken = default
    );
    Task DeleteAsync(int bookClubId, CancellationToken cancellationToken = default);
}
