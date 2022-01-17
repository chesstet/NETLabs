using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateLibrary.BLL.Common.Resources.Codes
{
    internal static class BookResourceCodes
    {
        internal const string UnrealToRetrieveAuthorError = nameof(UnrealToRetrieveAuthorError);
        internal const string UnrealToRetrieveDirectionError = nameof(UnrealToRetrieveDirectionError);
        internal const string UpdateBookError = nameof(UpdateBookError);
        internal const string UpdateBookDirectionError = nameof(UpdateBookDirectionError);
        internal const string UpdateBookAuthorError = nameof(UpdateBookAuthorError);
        internal const string UpdateBookCustomerError = nameof(UpdateBookCustomerError);
        internal const string CustomerNotFoundError = nameof(CustomerNotFoundError);
        internal const string BookForCustomerAlreadyExistsError = nameof(BookForCustomerAlreadyExistsError);
        internal const string BookForCustomerNotFoundError = nameof(BookForCustomerNotFoundError);
    }
}
