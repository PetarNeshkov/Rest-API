﻿using Microsoft.AspNetCore.Mvc;
using Models.Books;
using Models.Errors;
using Services.Books.Interfaces;
using static Common.GlobalConstants.ErrorMessages;

namespace Web.Controllers;

public class BooksController(IBooksBusinessService booksBusinessService)
    : BaseApiController
{
    [HttpGet]
    public async Task<IActionResult> GetAll(int page)
    {
        if (page < 1)
        {
            return BadRequest(new ErrorResponse { Message = InvalidPageNumberMessage });
        }

        var result = await booksBusinessService.GetBooksByPage(page);

        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetBookData(Guid id)
    {
        var result = await booksBusinessService.GetBookData(id);

        if (result == null)
        {
            return NotFound(new ErrorResponse { Message = BookDoesNotExistMessage });
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> AddBook(CreateBookRequestModel book)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var result = await booksBusinessService.CreateNewBook(book);

        return Ok(result);
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateBook(EditRequestModel model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var book = await booksBusinessService.UpdateBook(model);
        if (book == null)
        {
            return NotFound(new ErrorResponse{ Message = BookDoesNotExistMessage });
        }

        return Ok(book);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteBook(Guid id)
    {
        var result = await booksBusinessService.DeleteBook(id);

        if (result == null)
        {
            return NotFound(new ErrorResponse{ Message = BookDoesNotExistMessage });
        }

        return Ok(result);
    }
}