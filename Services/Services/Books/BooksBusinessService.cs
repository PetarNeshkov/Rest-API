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

    public async Task<string?> DeleteBook(Guid id)
    {
        var bookToDelete = await libraryDbContext.Books.FindAsync(id);

        if (bookToDelete == null)
        {
            return null;
        }
        
        var successfulMessage = string.Format(SuccessfulDeleteMessage, bookToDelete.Title);

        libraryDbContext.Remove(bookToDelete!);
        await libraryDbContext.SaveChangesAsync();

        return successfulMessage;
    }
}