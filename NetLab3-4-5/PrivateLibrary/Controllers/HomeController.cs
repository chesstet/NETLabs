using Microsoft.AspNetCore.Mvc;

namespace PrivateLibrary.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
