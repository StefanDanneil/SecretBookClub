namespace Contracts;

public record BookClubDto(int Id, string Name, ICollection<UserDto> Members);

public record BookClubForCreationDto(string Name);

public record BookClubForUpdateDto(string Name);
