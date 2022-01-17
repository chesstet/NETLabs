using System.Globalization;
using System.Resources;

namespace PrivateLibrary.BLL.Common.Resources.References
{
    /// <summary>
    /// Base class for a resource instance.
    /// </summary>
    internal abstract class Resource
    {
        private readonly ResourceManager _resourceManager;

        protected Resource(ResourceManager? resourceManager)
        {
            _resourceManager = resourceManager ?? throw new ArgumentNullException(nameof(resourceManager));
        }
        
        /// <summary>
        /// Gets essential string from resources by the given resourceKey.
        /// </summary>
        /// <param name="resourceKey">The key for search in resources</param>
        /// <param name="culture">Provides information about a specific culture.</param>
        /// <returns>The <see cref="String"/> value that is taken from resources. If value is not found, it returns null.</returns>
        protected string? GetResourceString(string resourceKey, CultureInfo? culture = null)
        {
            return _resourceManager.GetString(resourceKey, culture);
        }
    }
}
