using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Config.RequestLimits;
using PrivateLibrary.DAL.Utils.Enums.Security.Roles;
using PrivateLibrary.Models;
using PrivateLibrary.Models.Books;
using PrivateLibrary.Util.Enums.Security.Roles;
using PrivateLibrary.ViewModels.Books;

namespace PrivateLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IBookManagerService _bookManagerService;

        public BookController(IMapper mapper, IBookManagerService bookManagerService)
        {
            _mapper = mapper;
            _bookManagerService = bookManagerService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            var book = await _bookManagerService.GetByIdAsync(id);

            if (book != null) return View(_mapper.Map<BookModel>(book));

            ModelState.AddModelError(string.Empty, "Book was not found.");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetByAuthorId(Guid authorId)
        {
            var books = await _bookManagerService.GetByAuthorIdAsync(authorId);

            if (books != null) return View("BookList", new BookListViewModel{SearchResultModel = _mapper.Map<SearchResult<ShortBookModel>>(books), BookFilterModel = new BookFilter()});

            ModelState.AddModelError(string.Empty, "Unable to find books for this author.");
            return View("BookList");
        }

        [HttpGet]
        public async Task<IActionResult> GetByLibraryCustomerId(Guid customerId)
        {
            var books = await _bookManagerService.GetByLibraryCustomerIdAsync(customerId);

            if (books != null) return View("BookList", new BookListViewModel { SearchResultModel = _mapper.Map<SearchResult<ShortBookModel>>(books), BookFilterModel = new BookFilter() });

            ModelState.AddModelError(string.Empty, "Unable to find books for this customer id.");
            return View("BookList");
        }

        [HttpGet]
        public async Task<IActionResult> GetByFilter([FromQuery] BookFilter bookFilter)
        {
            var results = await _bookManagerService.GetByFilterAsync(_mapper.Map<ShortBookFilterDto>(bookFilter));

            return View("BookList", new BookListViewModel{SearchResultModel = _mapper.Map<SearchResult<ShortBookModel>>(results), BookFilterModel = bookFilter});
        }

        [Authorize(Roles = LibraryRole.Admin)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = LibraryRole.Admin)]
        [HttpPost]
        public async Task<IActionResult> Create(BookModel bookModel)
        {
            if (!ModelState.IsValid) return View();

            var newBookResult = await _bookManagerService.CreateAsync(_mapper.Map<BookDto>(bookModel));

            if (newBookResult is { Succeeded: true })
            {
                return RedirectToAction(nameof(Get), new { Id = newBookResult.Value?.Id });
            }

            ModelState.AddModelError(string.Empty, newBookResult.OperationResult.Errors.FirstOrDefault()?.Description!);
            return View();
        }

        [Authorize(Roles = LibraryRoleUnit.Admin)]
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var book = await _bookManagerService.GetByIdAsync(id);

            if (book != null) return View(book);

            ModelState.AddModelError(string.Empty, "Book was not found.");
            return View();
        }

        [Authorize(Roles = LibraryRoleUnit.Admin)]
        [HttpPost]
        public async Task<IActionResult> Edit(BookModel bookModel)
        {
            if (!ModelState.IsValid) return View();

            var updatedBookResult = await _bookManagerService.UpdateAsync(_mapper.Map<BookDto>(bookModel));

            if (updatedBookResult is { Succeeded: true })
            {
                return RedirectToAction(nameof(Get), new {Id = updatedBookResult.Value?.Id});
            }

            ModelState.AddModelError(string.Empty, updatedBookResult.OperationResult.Errors.FirstOrDefault()?.Description!);
            return View();
        }

        [Authorize(Roles = LibraryRoleUnit.Admin)]
        [HttpGet]
        public async Task<IActionResult> Remove(Guid id)
        {
            var book = await _bookManagerService.GetByIdAsync(id);

            if (book != null) return View(book);

            ModelState.AddModelError(string.Empty, "Book was not found.");
            return View();
        }

        [Authorize(Roles = LibraryRoleUnit.Admin)]
        [HttpPost]
        public async Task<IActionResult> Remove(ShortBookModel shortBookModel)
        {
            if (!ModelState.IsValid) return View();

            await _bookManagerService.RemoveAsync(shortBookModel.Id);
            return RedirectToAction(shortBookModel.ReturnUrl);
        }
    }
}
