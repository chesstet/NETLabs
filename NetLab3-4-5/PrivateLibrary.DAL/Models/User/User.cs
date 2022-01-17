using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace PrivateLibrary.DAL.Models.User
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
