namespace Domain.Entities;

public class BookClub : IEntity
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public ICollection<User> Members { get; set; } = [];
}
