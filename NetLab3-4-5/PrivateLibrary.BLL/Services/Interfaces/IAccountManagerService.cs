using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using PrivateLibrary.BLL.Dtos.Account;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    /// <summary>
    /// Used to manage user accounts.
    /// </summary>
    public interface IAccountManagerService
    {
        /// <summary>
        /// Creates a new user account.
        /// </summary>
        /// <param name="userCreationDto"></param>
        /// <returns></returns>
        public Task<IdentityResult> CreateUserAsync(UserCreationDto userCreationDto);
        /// <summary>
        /// Check if a user with the given login already exists.
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task<bool> IsUserWithSuchLoginAlreadyExist(string? login);
        /// <summary>
        /// Searches a user by his id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UserDto> FindByIdAsync(string? id);
        /// <summary>
        /// Searches a user by his login. 
        /// </summary>
        /// <param name="login"></param>
        /// <returns></returns>
        public Task<UserDto?> FindByLoginAsync(string? login);

        /// <summary>
        /// Adds a user to the specific role.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<IdentityResult> AddToRoleAsync(string? userEmail, string role);

        /// <summary>
        /// Removes a user from the specific role.
        /// </summary>
        /// <param name="userEmail"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public Task<IdentityResult> RemoveFromRoleAsync(string? userEmail, string role);

        public Task<IdentityResult> AddClaimAsync(string? userEmail, string claimType, string claimValue);

        public Task<IEnumerable<Claim>> GetClaimsAsync(string? login);
    }
}
