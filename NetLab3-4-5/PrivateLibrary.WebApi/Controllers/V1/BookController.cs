using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Models.Books;
using PrivateLibrary.WebApi.Models;
using PrivateLibrary.WebApi.Models.Books;
using PrivateLibrary.WebApi.Util.Enums.Security.Roles;

namespace PrivateLibrary.WebApi.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IBookManagerService _bookManagerService;
        public BookController(IMapper mapper, IBookManagerService bookManagerService)
        {
            _mapper = mapper;
            _bookManagerService = bookManagerService;
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _bookManagerService.GetByIdAsync(id);
            if (book != null) return Ok(_mapper.Map<BookModel>(book));

            return BadRequest("Book was not found.");
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShortBookModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetByAuthorId(Guid authorId)
        {
            var books = await _bookManagerService.GetByAuthorIdAsync(authorId);

            if (books != null) return Ok(_mapper.Map<ShortBookModel>(books));

            return BadRequest("Unable to find books for this author.");
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<ShortBookDto>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetByLibraryCustomerId(Guid customerId)
        {
            var books = await _bookManagerService.GetByLibraryCustomerIdAsync(customerId);

            if (books != null) return Ok(_mapper.Map<ShortBookModel>(books));

            return BadRequest("Unable to find books for this customer id.");
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(SearchResult<ShortBookModel>))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] BookFilter bookFilter)
        {
            var results = await _bookManagerService.GetByFilterAsync(_mapper.Map<ShortBookFilterDto>(bookFilter));

            return Ok(_mapper.Map<SearchResult<ShortBookModel>>(results));
        }


        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = LibraryRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(BookModel bookModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var newBookResult = await _bookManagerService.CreateAsync(_mapper.Map<BookDto>(bookModel));

            if (newBookResult is { Succeeded: true })
            {
                return CreatedAtAction(nameof(Get), new { Id = newBookResult.Value?.Id });
            }

            return BadRequest(newBookResult.OperationResult.Errors.FirstOrDefault()?.Description!);
        }

        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(BookModel))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = LibraryRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(BookModel bookModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            var updatedBookResult = await _bookManagerService.UpdateAsync(_mapper.Map<BookDto>(bookModel));

            if (updatedBookResult is { Succeeded: true })
            {
                return Ok(_mapper.Map<BookModel>(updatedBookResult));
            }

            return BadRequest(updatedBookResult.OperationResult.Errors.FirstOrDefault()?.Description!);
        }


        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = LibraryRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> Remove(ShortBookModel shortBookModel)
        {
            if (!ModelState.IsValid) return BadRequest();

            await _bookManagerService.RemoveAsync(shortBookModel.Id);
            return Ok();
        }
    }
}
