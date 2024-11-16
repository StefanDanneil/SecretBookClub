using Contracts;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Repositories;
using Mapster;
using Services.Abstractions;

namespace Services;

internal sealed class BookClubService(IRepositoryManager repositoryManager) : IBookClubService
{
    public async Task<IEnumerable<BookClubDto>> GetAllAsync(
        CancellationToken cancellationToken = default
    )
    {
        var bookClubs = await repositoryManager.BookClubRepository.GetAllAsync(cancellationToken);
        var bookClubsDto = bookClubs.Adapt<IEnumerable<BookClubDto>>();
        return bookClubsDto;
    }

    public async Task<BookClubDto> GetByIdAsync(
        int bookClubId,
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
        var bookClubDto = bookClub.Adapt<BookClubDto>();
        return bookClubDto;
    }

    public async Task<BookClubDto> CreateAsync(
        BookClubForCreationDto bookClubForCreationDto,
        CancellationToken cancellationToken = default
    )
    {
        var bookClub = bookClubForCreationDto.Adapt<BookClub>();
        repositoryManager.BookClubRepository.Insert(bookClub);
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        return bookClub.Adapt<BookClubDto>();
    }

    public async Task<BookClubDto> UpdateAsync(
        int bookClubId,
        BookClubForUpdateDto bookClubForUpdateDto,
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
        bookClub.Name = bookClubForUpdateDto.Name;
        await repositoryManager.UnitOfWork.SaveChangesAsync(cancellationToken);
        return bookClub.Adapt<BookClubDto>();
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
