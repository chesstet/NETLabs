using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Common.Resources;
using PrivateLibrary.BLL.Common.Resources.Codes;
using PrivateLibrary.BLL.Dtos.Account;
using PrivateLibrary.BLL.Dtos.Auth;
using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Infrastructure.Enums.Security.Claims;
using PrivateLibrary.BLL.Infrastructure.Enums.Security.Roles;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.DAL.Models.User;

namespace PrivateLibrary.BLL.Services.Realizations
{
    /// <summary>
    /// Used to authenticate, authorize and register users.
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<AuthService> _logger;
        private readonly SignInManager<User> _signInManager;
        private readonly IAccountManagerService _accountManagerService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IMapper mapper, ILogger<AuthService> logger, SignInManager<User> signInManager, IAccountManagerService accountManagerService, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;  
            _logger = logger;
            _signInManager = signInManager;
            _accountManagerService = accountManagerService;
            _httpContextAccessor = httpContextAccessor;
        }
        ///<inheritdoc/>
        public async Task<SignInResult> LoginAsync(LoginDto loginDto, bool lockoutOnFailure = false)
        {
            ArgumentNullException.ThrowIfNull(loginDto, nameof(loginDto));

            _logger.LogDebug($"Logging user with login = {loginDto.Login}.");
            await SignOutAsync();
            return await _signInManager.PasswordSignInAsync(loginDto.Login, loginDto.Password, loginDto.Remember,
                lockoutOnFailure);
        }
        ///<inheritdoc/>
        public async Task<OperationResult> RegisterAsync(RegisterDto registerDto)
        {
            ArgumentNullException.ThrowIfNull(registerDto, nameof(registerDto));

            _logger.LogDebug($"Started registering a new user with login = {registerDto.Login}.");
            var isUserAlreadyExist = await _accountManagerService.IsUserWithSuchLoginAlreadyExist(registerDto.Login);
            if (isUserAlreadyExist)
            {
                _logger.LogWarning($"Prevented action! User with login = {registerDto.Login} already exists.");
                return OperationResult.Failed(new OperationError{Code = AuthResourceCodes.UserWithSuchLoginAlreadyExistError, Description = Resources.RegistrationResource.UserWithSuchLoginAlreadyExistError});
            }

            var creationDto = _mapper.Map<UserCreationDto>(registerDto);
            var creationResult = await _accountManagerService.CreateUserAsync(creationDto);

            if (!creationResult.Succeeded)
            {
                _logger.LogError($"Unable to create a new user with given data, login = {registerDto.Login}.");
                return OperationResult.Failed(new OperationError { Code = AuthResourceCodes.CreatingUserError, Description = Resources.RegistrationResource.CreatingUserError });
            }

            var addToRoleResult = await _accountManagerService.AddToRoleAsync(creationDto.Login, LibraryRole.User);

            if (!addToRoleResult.Succeeded)
            {
                _logger.LogError($"Unable to create a new user with given data, login = {registerDto.Login}.");
                return OperationResult.Failed(new OperationError { Code = AuthResourceCodes.CreatingUserError, Description = Resources.RegistrationResource.CreatingUserError });
            }

            _logger.LogDebug($"Registering a new user with login = {registerDto.Login} was successfully finished.");
            return OperationResult.Success;
        }
        ///<inheritdoc/>
        public async Task SignOutAsync()
        {
            _logger.LogDebug("Signing the user out of the application.");
            await _signInManager.SignOutAsync();
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(string login, string? password)
        {
            ArgumentNullException.ThrowIfNull(login);
            ArgumentNullException.ThrowIfNull(password);
            var user = await _accountManagerService.FindByLoginAsync(login);

            if (user == null)
            {
                return SignInResult.Failed;
            }

            return await _signInManager.CheckPasswordSignInAsync(_mapper.Map<User>(user), password, false);
        }
    }
}
