using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.Models.Auth
{
    public class LoginModel : BaseModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? Login { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        public bool Remember { get; set; }
    }
}
