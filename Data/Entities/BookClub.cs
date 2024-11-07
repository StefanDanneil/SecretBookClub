using System.ComponentModel.DataAnnotations;

namespace Data.Entities;
public class BookClub
{
    public int Id { get; set; }
        
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
}