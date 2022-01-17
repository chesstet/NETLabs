using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Infrastructure.Enums.Security.Claims;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Config.RequestLimits;
using PrivateLibrary.DAL.Models.Book;
using PrivateLibrary.Models.Books;
using PrivateLibrary.Util.Enums.Security.Roles;

namespace PrivateLibrary.Controllers
{
    [Authorize]
    public class CustomerBooksController : Controller
    {
        private readonly IMapper _mapper;

        private readonly ICustomerBookService _customerBookService;

        private readonly BookLimits _bookLimitsOptions;
        public CustomerBooksController(IMapper mapper, ICustomerBookService customerBookService, IOptions<BookLimits> bookLimitsOptions)
        {
            _mapper = mapper;
            _customerBookService = customerBookService;
            _bookLimitsOptions = bookLimitsOptions.Value;
        }

        private Guid CustomerId { get; set; }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            CustomerId = new Guid(User.FindFirst(x => x.Type == ClaimType.LibraryCustomerId)!.Value);
        }

        [HttpGet]
        [Authorize(Roles = LibraryRoleUnit.Admin)]
        public async Task<IActionResult> GetByCustomerIdAsync(string id)
        {
            var result = await _customerBookService.GetByIdAsync(new Guid(id));

            return View("CustomerBooks",_mapper.Map<LibraryCustomerModel>(result));
        }

        [HttpGet]
        public async Task<IActionResult> GetCurrentSessionCustomerAsync()
        {
            var result = await _customerBookService.GetByIdAsync(CustomerId);

            return View("CustomerBooks", _mapper.Map<LibraryCustomerModel>(result));
        }

        [HttpPost]
        public async Task<IActionResult> AddBookForCurrentSessionCustomerAsync(Guid bookId)
        {
            var customerBooks = (await _customerBookService.GetByIdAsync(CustomerId))?.Books;
            if (customerBooks != null && customerBooks.Count < _bookLimitsOptions.Amount)
            {
                var result = await _customerBookService.AddBookForCustomer(CustomerId, bookId);

                if (!result.Succeeded)
                {
                    return BadRequest("Unreal to add a new book.");
                }

                return Ok();
            }

            return BadRequest("Unreal to add a new book.");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveBookForCurrentSessionCustomerAsync(Guid bookId)
        {
            var result = await _customerBookService.RemoveBookForCustomer(CustomerId, bookId);

            if (!result.Succeeded)
            {
                return BadRequest("Unreal to remove a book.");
            }

            return Ok();
        }
    }
}
