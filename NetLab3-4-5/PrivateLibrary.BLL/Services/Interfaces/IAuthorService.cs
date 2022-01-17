using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateLibrary.BLL.Dtos.Book;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    public interface IAuthorService : ICrudService<Guid, AuthorDto?>
    {
    }
}
