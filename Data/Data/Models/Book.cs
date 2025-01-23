using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Book
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public required string Title { get; set; }

    [Required]
    [MaxLength(200)]
    public required string Author { get; set; }

    [Required]
    public int PublishedYear { get; set; }

    [MaxLength(50)]
    public required string Genre { get; set; }
}