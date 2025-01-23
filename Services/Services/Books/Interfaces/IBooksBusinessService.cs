using Data.Models;
using Models.Books;
using Models.Pagination;

namespace Services.Books.Interfaces;

public interface IBooksBusinessService
{
    Task<PaginatedList<BookResponseModel>> GetBooksByPage(int page = 1);
    
    Task<BookResponseModel?> GetBookData(Guid bookId);
    
    Task<Book> CreateNewBook(CreateBookRequestModel model);

    Task<string> UpdateBook(EditRequestModel model);

    Task<string?> DeleteBook(Guid id);
}