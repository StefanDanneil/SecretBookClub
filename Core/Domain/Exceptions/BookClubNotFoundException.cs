namespace Domain.Exceptions;

public sealed class BookClubNotFoundException(int bookClubId)
    : NotFoundException($"The book club with the identifier {bookClubId} was not found.");