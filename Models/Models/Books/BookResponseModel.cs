namespace Models.Books;

public class BookResponseModel
{
    public Guid Id { get; init; }
    
    public required string Title { get; init; }
    
    public required string Author { get; init; }
    
    public int PublishedYear { get; init; }
    
    public required string Genre { get; init; }
}