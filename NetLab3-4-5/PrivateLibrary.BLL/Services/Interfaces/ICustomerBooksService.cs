using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateLibrary.BLL.Common;
using PrivateLibrary.BLL.Dtos.Book;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    public interface ICustomerBookService : ICrudService<Guid, LibraryCustomerDto?>
    {
        public Task<OperationResult> AddBookForCustomer(Guid customerId, Guid booId);

        public Task<OperationResult> RemoveBookForCustomer(Guid customerId, Guid booId);
    }
}
