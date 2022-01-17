using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PrivateLibrary.BLL.Dtos.Account;
using PrivateLibrary.BLL.Infrastructure.Enums.Security.Claims;
using PrivateLibrary.BLL.Infrastructure.Enums.Security.Roles;
using PrivateLibrary.BLL.Services.Interfaces;
using PrivateLibrary.DAL.Models.User;

namespace PrivateLibrary.BLL.Services.Realizations
{
    /// <summary>
    /// Used to manage user accounts.
    /// </summary>
    public class AccountManagerService : IAccountManagerService
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AccountManagerService> _logger;
        private readonly IMapper _mapper;
        public AccountManagerService(IMapper mapper, ILogger<AccountManagerService> logger, UserManager<User> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }
        ///<inheritdoc/>
        public async Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto)
        {
            ArgumentNullException.ThrowIfNull(userCreationDto);

            _logger.LogDebug($"Started creating new user with login {userCreationDto.Login}.");
            var user = new User
            {
                Name = userCreationDto.Name,
                Surname = userCreationDto.Surname,
                Email = userCreationDto.Login,
                UserName = userCreationDto.Login,
                EmailConfirmed = true // TODO: change this
            };

            var result = await _userManager.CreateAsync(user, userCreationDto.Password);
            if (!result.Succeeded)
            {
                _logger.LogDebug($"User with login = {userCreationDto.Login} was successfully created.");
            }

            return result;
        }

        public async Task<IdentityResult> AddToRoleAsync(string? userEmail, string role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(userEmail));
            }

            var roleAdditionResult = await _userManager.AddToRoleAsync(user, role);
            if (roleAdditionResult.Succeeded)
            {
                _logger.LogDebug($"User with email = {userEmail} was successfully added to the role {LibraryRole.User}.");
            }

            return roleAdditionResult;
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string? userEmail, string role)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(userEmail));
            }

            var roleRemovingResult = await _userManager.RemoveFromRoleAsync(user, role);

            if (roleRemovingResult.Succeeded)
            {
                _logger.LogDebug($"User with email = {userEmail} was successfully removed from the role {LibraryRole.User}.");
            }

            return roleRemovingResult;
        }

        public async Task<IdentityResult> AddClaimAsync(string? userEmail, string claimType, string claimValue)
        {
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user == null)
            {
                return IdentityResult.Failed(_userManager.ErrorDescriber.InvalidEmail(userEmail));
            }

            var claimAdditionResult = await _userManager.AddClaimAsync(user, new Claim(claimType, claimValue));
            if (claimAdditionResult.Succeeded)
            {
                _logger.LogDebug($"User with email = {userEmail} was successfully updated with adding given claims.");
            }

            return claimAdditionResult;
        }


        ///<inheritdoc/>
        public async Task<UserDto?> FindByLoginAsync(string? login)
        {
            var user = await _userManager.FindByEmailAsync(login);
            if (user == null) return null;
            _logger.LogDebug($"User with login = {login} was found.");
            return _mapper.Map<UserDto>(user);
        }
        ///<inheritdoc/>
        public async Task<bool> IsUserWithSuchLoginAlreadyExist(string? login)
        {
            return await _userManager.FindByEmailAsync(login) != null;
        }
        ///<inheritdoc/>
        public async Task<UserDto> FindByIdAsync(string? id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) _logger.LogDebug($"User with id = {id} was found.");
            return _mapper.Map<UserDto>(user);
        }

        public async Task<IEnumerable<Claim>> GetClaimsAsync(string? login)
        {
            var user = await _userManager.FindByEmailAsync(login);
            if (user == null)
            {
                throw new InvalidOperationException();
            }

            return await _userManager.GetClaimsAsync(user);
        }
    }
}
