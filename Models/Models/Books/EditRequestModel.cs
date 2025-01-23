using System.ComponentModel.DataAnnotations;

namespace Models.Books;
using static Common.GlobalConstants.ErrorMessages.Books;

public class EditRequestModel
{
    public Guid Id { get; init; }
    
    [Required(ErrorMessage = TitleIsRequiredMessage)]
    [MaxLength(200, ErrorMessage = TitleMaxLengthMessage)]
    public required string Title { get; init; }
    
    [Required(ErrorMessage = AuthorIsRequiredMessage)]
    [MaxLength(200, ErrorMessage = AuthorMaxLengthMessage)]
    public required string Author { get; init; }
    
    [Required(ErrorMessage = PublishedYearRequiredMessage)]
    public int PublishedYear { get; init; }
    
    [Required(ErrorMessage = GenreIsRequiredMessage)]
    [MaxLength(50, ErrorMessage = GenreMaxLengthRequiredMessage)]
    public required string Genre { get; init; }
}