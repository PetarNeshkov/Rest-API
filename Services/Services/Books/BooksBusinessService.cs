using System.ComponentModel.DataAnnotations;
using Data;
using Data.Models;
using Models.Books;
using Services.Books.Interfaces;
using static Common.GlobalConstants;

namespace Services.Books;

public class BooksBusinessService(LibraryDbContext libraryDbContext) : IBooksBusinessService
{
    public Task<BookResponseModel> GetCurrentBooks(int page = 1)
    {
       throw new NotImplementedException();
    }

    public async Task<string> CreateNewBook(CreateBookRequestModel model)
    {
        var book = new Book
        {
            Title = model.Title,
            Author = model.Author,
            Genre = model.Genre,
            PublishedYear = model.PublishedYear
        };
        
        libraryDbContext.Books.Add(book);
        await libraryDbContext.SaveChangesAsync();
        
        return string.Format(SuccessfulCreationMessage, model.Title);
    }

    public Task<string> UpdateBook(EditRequestModel model) => throw new NotImplementedException();

    public Task<string> DeleteBook(Guid id) => throw new NotImplementedException();
}