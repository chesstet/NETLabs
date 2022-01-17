using System.ComponentModel.DataAnnotations;

namespace PrivateLibraryFront.Util.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class RequireBoolValue : ValidationAttribute
    {
        public bool Required { get; init; }
        public override bool IsValid(object? value)
        {
            return value is bool val && val == Required;
        }
    }
}
