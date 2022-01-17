using Microsoft.AspNetCore.Identity;

namespace PrivateLibrary.BLL.Dtos.Account
{
    public class UserDto : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
