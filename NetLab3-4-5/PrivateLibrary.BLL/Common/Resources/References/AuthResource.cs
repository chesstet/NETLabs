using System.Resources;
using PrivateLibrary.BLL.Common.Resources.Codes;

namespace PrivateLibrary.BLL.Common.Resources.References
{
    internal class AuthResource : Resource
    {
        internal static readonly AuthResource Instance = new(ResourceManagers.RegistrationResourceManager);
        internal AuthResource(ResourceManager? resourceManager) 
            : base(resourceManager)
        {
        }

        internal string? CreatingUserError => GetResourceString(AuthResourceCodes.CreatingUserError);
        internal string? UserWithSuchLoginAlreadyExistError => GetResourceString(AuthResourceCodes.UserWithSuchLoginAlreadyExistError);
    }
}
