using Microsoft.AspNetCore.Identity;
using PrivateLibrary.DAL.Models.User;
using PrivateLibrary.DAL.Repositories.Interfaces;
using PrivateLibrary.DAL.Utils.Enums.Security.Roles;

namespace PrivateLibrary.DAL.Utils.Initializations
{
    /// <summary>
    /// Initializes base roles for the application.
    /// </summary>
    public static class RoleInitializer
    {
        /// <summary>
        /// This method must be used only in Development!
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <returns></returns>
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            const string adminEmail = "dayteE@gmail.com";
            const string adminPassword = "Qwerty123$";
            const string adminName = "Admin";
            const string adminSurname = "Surname";
            if (await roleManager.FindByNameAsync(LibraryRole.User) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(LibraryRole.User));
            }
            if (await roleManager.FindByNameAsync(LibraryRole.Admin) == null)
            {
                await roleManager.CreateAsync(new IdentityRole(LibraryRole.Admin));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail, Name = adminName , Surname = adminSurname};
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    result = await userManager.AddToRoleAsync(admin, LibraryRole.Admin);
                }

                if (!result.Succeeded) throw new InvalidOperationException();
            }
        }
    }
}
