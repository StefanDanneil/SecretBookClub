namespace Domain.Entities;

public class User : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<BookClub> BookClubs { get; set; } = [];
}
