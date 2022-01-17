using Microsoft.AspNetCore.Mvc;

namespace PrivateLibrary.WebApi.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet]
        public IActionResult Index()
        {
            return Ok();
        }
    }
}
