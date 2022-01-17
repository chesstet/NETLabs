using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PrivateLibrary.BLL.Common;
using PrivateLibraryFront.Configs;
using PrivateLibraryFront.Models;
using PrivateLibraryFront.Models.Auth;

namespace PrivateLibraryFront.Controllers
{
    public class AuthController : Controller
    {

        public AuthController()
        {

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
                using var client = new HttpClient();
                var response = await client.PostAsJsonAsync(UriReferences.WebApiV1 + "/Auth/Login", loginModel);
                var statusCode = response.StatusCode;

                if ((int)statusCode == StatusCodes.Status200OK)
                {
                    var token = (await response.Content.ReadAsAsync<TokenResp>()).GeneratedToken;
                    if (!string.IsNullOrEmpty(token))
                    {
                        HttpContext.Session.SetString("Token", token);
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError(string.Empty, "Something wrong has happened.");
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
            using var client = new HttpClient();
            var response = await client.PostAsJsonAsync(UriReferences.WebApiV1 + "/Auth/Register", registerModel);
            var statusCode = response.StatusCode;

            if ((int)statusCode == StatusCodes.Status200OK)
            {
                return RedirectToAction(nameof(Login));
            }

            var errors = await response.Content.ReadAsAsync<IEnumerable<OperationError>>();
            errors.ToList().ForEach(x =>
            {
                if (x.Description != null) ModelState.AddModelError(string.Empty, x.Description);
            });
            return View();
        }

        [Authorize]
        public async Task<IActionResult> SignOutAsync()
        {
            using var client = new HttpClient();
            var response = await client.PostAsync(UriReferences.WebApiV1 + "/Auth/SignOutAsync", null);
            return RedirectToAction("Index", "Home");
        }
    }
}
