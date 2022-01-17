using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.WebApi.Config;
using PrivateLibrary.WebApi.Models.Auth;

namespace PrivateLibrary.WebApi.Controllers.V1
{
    [Route("api/v1/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IAccountManagerService _accountManagerService;
        private readonly TokenReqOptions _tokenOptions;

        public AuthController(IMapper mapper, IAuthService authService, ITokenService tokenService, IAccountManagerService accountManagerService, IOptions<TokenReqOptions> tokenOptions)
        {
            _mapper = mapper;
            _authService = authService;
            _tokenService = tokenService;
            _accountManagerService = accountManagerService;
            _tokenOptions = tokenOptions.Value;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> LoginAsync(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                var signInResult = await _authService.CheckPasswordSignInAsync(loginModel.Login, loginModel.Password);

                if (!signInResult.Succeeded)
                {
                    return BadRequest("Data are not valid.");
                }

                var claims = await _accountManagerService.GetClaimsAsync(loginModel.Login);
                var generatedToken = _tokenService.BuildToken(_tokenOptions.Key, _tokenOptions.Issuer, _tokenOptions.DurationMinutes,_mapper.Map<LoginDto>(loginModel), claims);
                if (generatedToken != null)
                {
                    HttpContext.Session.SetString("Token", generatedToken);
                    var resp = new { GeneratedToken = generatedToken };
                    return Ok(resp);
                }
                return BadRequest("Unable to log in.");
            }

            return BadRequest();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> RegisterAsync(RegisterModel registerModel)
        {
            if (!ModelState.IsValid) return BadRequest();
            var operationResult = await _authService.RegisterAsync(_mapper.Map<RegisterDto>(registerModel));
            if (!operationResult.Succeeded)
            {
                return BadRequest(operationResult.Errors);
            }

            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SignOutAsync()
        {
            await _authService.SignOutAsync();
            return Ok();
        }
    }
}
