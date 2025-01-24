using Data;
using Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Books;
using Models.Errors;
using Models.Pagination;
using Moq;
using Services.Books;
using Services.Books.Interfaces;
using Web.Controllers;
using Xunit;
using static Common.GlobalConstants;
using static Common.GlobalConstants.ErrorMessages;

namespace Tests.Books;

public class BooksBusinessServiceTests
{
    private readonly Mock<IBooksBusinessService> booksBusinessService;
    private readonly BooksController controller;

    public BooksBusinessServiceTests()
    {
        booksBusinessService = new Mock<IBooksBusinessService>();
        controller = new BooksController(booksBusinessService.Object);
    }
    
    [Fact]
    public async Task GetBooksByPage_ShouldReturnPaginatedBooks()
    {
        var page = 1;
        var booksResult = new PaginatedList<BookResponseModel>
        {
            Page = page,
            PageSize = 10,
            TotalCount = 2,
            TotalPages = 1,
            Items =
            [
                new BookResponseModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Book 1",
                    Author = "Author 1",
                    Genre = "Fiction",
                    PublishedYear = 2000
                },

                new BookResponseModel
                {
                    Id = Guid.NewGuid(),
                    Title = "Book 2",
                    Author = "Author 2",
                    Genre = "Non-Fiction",
                    PublishedYear = 2010
                }
            ]
        };

        booksBusinessService
            .Setup(service => service.GetBooksByPage(page))
            .ReturnsAsync(booksResult);

        var result = await controller.GetAll(page);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PaginatedList<BookResponseModel>>(okResult.Value);
        Assert.Equal(2, response.Items!.Count);
        Assert.Equal(2, response.TotalCount);
        Assert.Equal("Book 1", response.Items[0].Title);
        Assert.Equal("Book 2", response.Items[1].Title);
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnBadRequest_WhenPageIsNegative()
    {
        const int negativePage = -1;

        var result = await controller.GetAll(negativePage);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

        var response = Assert.IsType<ErrorResponse>(badRequestResult.Value);

        Assert.Equal(InvalidPageNumberMessage, response.Message);
    }
    
    [Fact]
    public async Task GetAll_ShouldReturnEmptyList_WhenNoBooksExist()
    {
        var validPage = 1;
        var emptyResult = new PaginatedList<BookResponseModel>
        {
            Page = validPage,
            PageSize = 10,
            TotalCount = 0,
            TotalPages = 0,
            Items = []
        };

        booksBusinessService
            .Setup(service => service.GetBooksByPage(validPage))
            .ReturnsAsync(emptyResult);

        var result = await controller.GetAll(validPage);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<PaginatedList<BookResponseModel>>(okResult.Value);
        Assert.Empty(response.Items!);
        Assert.Equal(0, response.TotalCount);
    }
    
    [Fact]
    public async Task GetBookData_ShouldReturnBook_WhenBookExists()
    {
        var bookId = Guid.NewGuid();
        var mockBook = new BookResponseModel
        {
            Id = bookId,
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction",
            PublishedYear = 2022
        };

        booksBusinessService
            .Setup(service => service.GetBookData(bookId))
            .ReturnsAsync(mockBook);

        var result = await controller.GetBookData(bookId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<BookResponseModel>(okResult.Value);

        Assert.Equal(mockBook.Id, response.Id);
        Assert.Equal(mockBook.Title, response.Title);
        Assert.Equal(mockBook.Author, response.Author);
        Assert.Equal(mockBook.Genre, response.Genre);
        Assert.Equal(mockBook.PublishedYear, response.PublishedYear);
    }
    
    [Fact]
    public async Task GetBookData_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        var bookId = Guid.NewGuid();
        
        booksBusinessService
            .Setup(service => service.GetBookData(bookId))
            .ReturnsAsync((BookResponseModel)null!);

        var result = await controller.GetBookData(bookId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        var response = Assert.IsType<ErrorResponse>(notFoundResult.Value);

        Assert.Equal(BookDoesNotExistMessage, response.Message);
    }

    [Fact]
    public async Task AddBook_ShouldReturnOk_WhenBookIsCreated()
    {
        var mockBookRequest = new CreateBookRequestModel
        {
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction",
            PublishedYear = 2022
        };

        var mockBook = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction",
            PublishedYear = 2022
        };

        booksBusinessService
            .Setup(service => service.CreateNewBook(mockBookRequest))
            .ReturnsAsync(mockBook);

        var result = await controller.AddBook(mockBookRequest);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<Book>(okResult.Value);

        Assert.Equal(mockBook.Title, response.Title);
        Assert.Equal(mockBook.Author, response.Author);
        Assert.Equal(mockBook.Genre, response.Genre);
        Assert.Equal(mockBook.PublishedYear, response.PublishedYear);
    }
    
    [Fact]
    public async Task AddBook_ShouldReturnBadRequest_WhenValidationFails()
    {
        var invalidBookRequest = new CreateBookRequestModel
        {
            Title = "",
            Author = "Test Author",
            Genre = "Fiction",
            PublishedYear = 2022
        };
    
        controller.ModelState.AddModelError("Title", "The Title field is required.");
    
        var result = await controller.AddBook(invalidBookRequest);
    
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<SerializableError>(badRequestResult.Value);
    
        Assert.True(response.ContainsKey("Title"));
        Assert.Equal("The Title field is required.", ((string[])response["Title"])[0]);
    }
    
    [Fact]
    public async Task UpdateBook_ShouldReturnBadRequest_WhenValidationFails()
    {
        var invalidEditRequest = new EditRequestModel
        {
            Id = Guid.NewGuid(),
            Title = "",
            Genre = "New Genre",
            Author = "Test Author"
        };

        controller.ModelState.AddModelError("Title", "The Title field is required.");

        var result = await controller.UpdateBook(invalidEditRequest);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        var response = Assert.IsType<SerializableError>(badRequestResult.Value);

        Assert.True(response.ContainsKey("Title"));
        Assert.Equal("The Title field is required.", ((string[])response["Title"])[0]);
    }
    
    [Fact]
    public async Task UpdateBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        var editRequest = new EditRequestModel
        {
            Id = Guid.NewGuid(),
            Title = "Updated Title",
            Author = "Updated Author",
            Genre = "Updated Genre",
            PublishedYear = 2023
        };

        booksBusinessService
            .Setup(service => service.UpdateBook(editRequest))
            .ReturnsAsync((string?)null);

        var result = await controller.UpdateBook(editRequest);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        var response = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        Assert.Equal(BookDoesNotExistMessage, response.Message);
    }
    
    [Fact]
    public async Task UpdateBook_ShouldReturnOk_WhenBookIsSuccessfullyUpdated()
    {
        var editRequest = new EditRequestModel
        {
            Id = Guid.NewGuid(),
            Title = "Updated Title",
            Author = "Updated Author",
            Genre = "Updated Genre",
            PublishedYear = 2023
        };
        
        var successMessage = string.Format(SuccessfulEditMessage, editRequest.Id);

        booksBusinessService
            .Setup(service => service.UpdateBook(editRequest))
            .ReturnsAsync(successMessage);

        var result = await controller.UpdateBook(editRequest);
        var okResult = Assert.IsType<OkObjectResult>(result);

        var response = Assert.IsType<string>(okResult.Value);
        Assert.Equal(successMessage, response);
    }
    
    [Fact]
    public async Task DeleteBook_ShouldReturnOk_WhenBookIsSuccessfullyDeleted()
    {
        var bookId = Guid.NewGuid();
        var successfulMessage = string.Format(SuccessfulDeleteMessage, bookId);

        booksBusinessService
            .Setup(service => service.DeleteBook(bookId))
            .ReturnsAsync(successfulMessage);

        var result = await controller.DeleteBook(bookId);

        var okResult = Assert.IsType<OkObjectResult>(result);

        var response = Assert.IsType<string>(okResult.Value);
        Assert.Equal(successfulMessage, response);
    }
    
    [Fact]
    public async Task DeleteBook_ShouldReturnNotFound_WhenBookDoesNotExist()
    {
        var bookId = Guid.NewGuid();

        booksBusinessService
            .Setup(service => service.DeleteBook(bookId))
            .ReturnsAsync((string?)null);

        var result = await controller.DeleteBook(bookId);

        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

        var response = Assert.IsType<ErrorResponse>(notFoundResult.Value);
        Assert.Equal(BookDoesNotExistMessage, response.Message);
    }
    
    [Fact]
    public async Task DeleteBook_ShouldDeleteBook_WhenBookExists()
    {
        // Arrange
        var dbOptions = new DbContextOptionsBuilder<LibraryDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        var dbContext = new LibraryDbContext(dbOptions);
        var service = new BooksBusinessService(dbContext);

        var bookToDelete = new Book
        {
            Id = Guid.NewGuid(),
            Title = "Test Book",
            Author = "Test Author",
            Genre = "Fiction",
            PublishedYear = 2022
        };

        dbContext.Books.Add(bookToDelete);
        await dbContext.SaveChangesAsync();

        var result = await service.DeleteBook(bookToDelete.Id);

        Assert.NotNull(result);
        var successfulMessage = string.Format(SuccessfulDeleteMessage, bookToDelete.Title);

        Assert.Equal(successfulMessage, result);

        var deletedBook = await dbContext.Books.FindAsync(bookToDelete.Id);
        Assert.Null(deletedBook);
    }
}