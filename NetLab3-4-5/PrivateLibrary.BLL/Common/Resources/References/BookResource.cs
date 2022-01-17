using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using PrivateLibrary.BLL.Common.Resources.Codes;

namespace PrivateLibrary.BLL.Common.Resources.References
{
    internal class BookResource : Resource
    {
        internal static readonly BookResource Instance = new(ResourceManagers.BookResourceManager);
        internal BookResource(ResourceManager? resourceManager)
            : base(resourceManager)
        {
        }

        internal string? UnrealToRetrieveAuthorError => GetResourceString(BookResourceCodes.UnrealToRetrieveAuthorError);
        internal string? UnrealToRetrieveDirectionError => GetResourceString(BookResourceCodes.UnrealToRetrieveDirectionError);
        internal string? UpdateBookError => GetResourceString(BookResourceCodes.UpdateBookError);
        internal string? UpdateBookAuthorError => GetResourceString(BookResourceCodes.UpdateBookAuthorError);
        internal string? UpdateBookDirectionError => GetResourceString(BookResourceCodes.UpdateBookDirectionError);
        internal string? UpdateBookCustomerError => GetResourceString(BookResourceCodes.UpdateBookCustomerError);
        internal string? CustomerNotFoundError => GetResourceString(BookResourceCodes.CustomerNotFoundError);
        internal string? BookForCustomerAlreadyExistsError => GetResourceString(BookResourceCodes.BookForCustomerAlreadyExistsError);
        internal string? BookForCustomerNotFoundError => GetResourceString(BookResourceCodes.BookForCustomerNotFoundError);
    }
}
