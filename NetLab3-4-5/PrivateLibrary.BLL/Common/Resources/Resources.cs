using PrivateLibrary.BLL.Common.Resources.References;

namespace PrivateLibrary.BLL.Common.Resources
{
    /// <summary>
    /// Provides access to all resource instances.
    /// </summary>
    internal static class Resources
    {
        internal static AuthResource RegistrationResource => AuthResource.Instance;
        internal static BookResource BookResource => BookResource.Instance;
    }
}
