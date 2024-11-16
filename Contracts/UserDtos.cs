namespace Contracts;

public record UserDto(int Id, string Name, ICollection<BookClubDto> BookClubs);

public record UserForCreationDto(string Name);

public class UserForUpdateDto(string Name);
