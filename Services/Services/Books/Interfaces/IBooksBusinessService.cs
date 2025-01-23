using Models.Books;

namespace Services.Books.Interfaces;

public interface IBooksBusinessService
{
    Task<BookResponseModel> GetCurrentBooks(int page);
    
    Task<string> CreateNewBook(CreateBookRequestModel model);

    Task<string> UpdateBook(EditRequestModel model);

    Task<string> DeleteBook(Guid id);
}