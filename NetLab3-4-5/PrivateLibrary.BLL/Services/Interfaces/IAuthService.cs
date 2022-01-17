using Microsoft.AspNetCore.Identity;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Dtos.Auth;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    /// <summary>
    /// Used to authenticate, authorize and register users.
    /// </summary>
    public interface IAuthService
    {
        /// <summary>
        /// Authenticate user by given <see cref="LoginDto"/> and lockoutOnFailure flag.
        /// </summary>
        /// <param name="loginDto"></param>
        /// <param name="lockoutOnFailure"></param>
        /// <returns></returns>
        public Task<SignInResult> LoginAsync(LoginDto loginDto, bool lockoutOnFailure = false);
        /// <summary>
        /// Register new user by given <see cref="RegisterDto"/>.
        /// </summary>
        /// <param name="registerDto"></param>
        /// <returns></returns>
        public Task<OperationResult> RegisterAsync(RegisterDto registerDto);
        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <returns></returns>
        public Task SignOutAsync();

        public Task<SignInResult> CheckPasswordSignInAsync(string? login, string? password);
    }
}
