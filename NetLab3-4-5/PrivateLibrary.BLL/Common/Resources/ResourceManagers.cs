using System.Resources;

namespace PrivateLibrary.BLL.Common.Resources
{
    /// <summary>
    /// Provides access to all resource managers.
    /// </summary>
    internal static class ResourceManagers
    {
        private static ResourceManager? _registrationResourceManager;
        private static ResourceManager? _bookResourceManager;

        internal static ResourceManager RegistrationResourceManager => _registrationResourceManager ??= AllResources.Auth.AuthResource.ResourceManager;
        internal static ResourceManager BookResourceManager => _bookResourceManager ??= AllResources.Books.BookResource.ResourceManager;
    }
}
