using Microsoft.AspNetCore.Mvc;
using PrivateLibraryFront.Models;
using System.Diagnostics;
using PrivateLibrary.WebApi.Models.Books;
using PrivateLibraryFront.Configs;

namespace PrivateLibraryFront.Controllers
{
    public class BookController : Controller
    {
        [HttpGet]
        public IActionResult BookList()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Get(Guid id)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(UriReferences.WebApiV1 + "/Book/Get?id=" + id);
            var content = await response.Content.ReadAsAsync<BookModel>();

            return content == null ? View("Error") : View("Get", content);
        }

        [HttpGet]
        public async Task<IActionResult> GetByCustomerId(Guid id)
        {
            using var client = new HttpClient();
            var response = await client.GetAsync(UriReferences.WebApiV1 + "/Book/GetByLibraryCustomerId?id=" + id);
            var content = await response.Content.ReadAsAsync<ShortBookModel>();

            return Ok(content);
        }
    }
}
