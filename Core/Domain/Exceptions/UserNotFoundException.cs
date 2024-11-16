namespace Domain.Exceptions;

public sealed class UserNotFoundException(int userId)
    : NotFoundException($"The user with the identifier {userId} was not found.");