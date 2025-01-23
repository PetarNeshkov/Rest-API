using System.ComponentModel.DataAnnotations;
using Data;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Models.Books;
using Models.Pagination;
using Services.Books.Interfaces;
using static Common.GlobalConstants;

namespace Services.Books;

public class BooksBusinessService(LibraryDbContext libraryDbContext) : IBooksBusinessService
{
    
    public async Task<PaginatedList<BookResponseModel>> GetBooksByPage(int page = 1)
    {
        var skip = (page - 1) * BooksPerPage;

        var totalBooks = await libraryDbContext.Books.CountAsync();

        var books = await libraryDbContext.Books
            .Skip(skip)
            .Take(BooksPerPage)
            .Select(b => new BookResponseModel
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Genre = b.Genre,
                PublishedYear = b.PublishedYear
            })
            .ToListAsync();
        
        return new PaginatedList<BookResponseModel>
        {
            Page = page,
            PageSize = BooksPerPage,
            TotalCount = totalBooks,
            TotalPages = (int)Math.Ceiling(totalBooks / (double)BooksPerPage),
            Items = books
        };
    }

    public async Task<BookResponseModel?> GetBookData(Guid bookId)
    {
        var book = await libraryDbContext.Books.FindAsync(bookId);

        if (book == null)
        {
            return null;
        }

        return new BookResponseModel
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Genre = book.Genre,
            PublishedYear = book.PublishedYear,
        };
    }

    public async Task<Book> CreateNewBook(CreateBookRequestModel model)
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
        
        return book;
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