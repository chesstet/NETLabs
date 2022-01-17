using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateLibrary.BLL.Dtos.Account;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Infrastructure.Enums.Security.Claims;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.Models.Auth;

namespace PrivateLibrary.Controllers
{
    public class AuthController : Controller
    {
        private readonly IMapper _mapper;

        private readonly IAuthService _authService;
        private readonly ICustomerBookService _customerBookService;
        private readonly IAccountManagerService _accountManagerService;

        public AuthController(IMapper mapper, IAuthService authService, ICustomerBookService customerBookService, IAccountManagerService accountManagerService)
        {
            _mapper = mapper;
            _authService = authService;
            _customerBookService = customerBookService;
            _accountManagerService = accountManagerService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _authService.LoginAsync(_mapper.Map<LoginDto>(loginModel));

                if (signInResult.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
        {
            if (!ModelState.IsValid) return View();
            var operationResult = await _authService.RegisterAsync(_mapper.Map<RegisterDto>(registerModel));
            if (!operationResult.Succeeded)
            {
                operationResult.Errors.ToList().ForEach(x =>
                {
                    if (x.Description != null) ModelState.AddModelError(string.Empty, x.Description);
                });
                return View();
            }

            var user = await _accountManagerService.FindByLoginAsync(registerModel.Login);

            if (user == null)
            {
                return View("Error");
            }
            var libraryCustomerCreationResult =
                await _customerBookService.CreateAsync(new LibraryCustomerDto { UserId = new Guid(user.Id) });

            if (!libraryCustomerCreationResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Unable to create a library user.");
                return View();
            }

            var addClaimsResult = await _accountManagerService.AddClaimAsync(registerModel.Login, ClaimType.LibraryCustomerId, libraryCustomerCreationResult.Value!.Id.ToString());

            if (!addClaimsResult.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Unable to create a library user.");
                return View();
            }

            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public async Task<IActionResult> SignOutAsync()
        {
            await _authService.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
