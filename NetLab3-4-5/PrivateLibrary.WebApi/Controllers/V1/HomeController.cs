using Microsoft.AspNetCore.Mvc;
using PrivateLibrary.WebApi.Models.Auth;

namespace PrivateLibrary.WebApi.Controllers.V1
{

    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public IActionResult Index(LoginModel loginModel)
        {
            return Ok(loginModel.Login);
        }
    }
}
