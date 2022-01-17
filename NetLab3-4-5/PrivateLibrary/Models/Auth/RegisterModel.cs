using System.ComponentModel.DataAnnotations;
using PrivateLibrary.Util;
using PrivateLibrary.Util.Attributes;

namespace PrivateLibrary.Models.Auth
{
    public class RegisterModel : BaseModel
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string? Login { get; set; }
        [Required]
        [StringLength(30, MinimumLength = 1)]
        [RegularExpression(ViewConstants.NameValidationRegex, ErrorMessage = "Name contains invalid characters")]
        public string? Name { get; init; }
        [Required]
        [StringLength(30, MinimumLength = 1)]
        [RegularExpression(ViewConstants.NameValidationRegex, ErrorMessage = "Surname contains invalid characters")]
        public string? Surname { get; init; }
        [Required]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string? PhoneNumber { get; init; }
        [Required]
        [DataType(DataType.Password)]
        public string? Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string? RepeatPassword { get; set; }
        [Required]
        [RequireBoolValue(Required = true, ErrorMessage = "Please mark that you're agree with our agreements.")]
        public bool Agreement { get; set; }
    }
}
