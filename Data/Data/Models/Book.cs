using System.ComponentModel.DataAnnotations;

namespace Data.Models;

public class Book
{
    [Key]
    public Guid Id { get; init; } = Guid.NewGuid();

    [Required]
    [MaxLength(200)]
    public required string Title { get; init; }

    [Required]
    [MaxLength(200)]
    public required string Author { get; init; }

    [Required]
    public int PublishedYear { get; init; }

    [MaxLength(50)]
    public required string Genre { get; init; }
}