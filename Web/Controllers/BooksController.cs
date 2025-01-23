using Microsoft.AspNetCore.Mvc;
using Models.Books;
using Services.Books.Interfaces;
using static Common.GlobalConstants.ErrorMessages;

namespace Web.Controllers;

public class BooksController(IBooksBusinessService booksBusinessService)
    : BaseApiController
{
    [HttpPost]
    public async Task<IActionResult> AddBook(CreateBookRequestModel book)
    {
        var result = await booksBusinessService.CreateNewBook(book);

        if (string.IsNullOrWhiteSpace(result))
        {
            return BadRequest(new { Message = FailedCreationMessage });
        }

        return Ok(result);
    }
    
    
}