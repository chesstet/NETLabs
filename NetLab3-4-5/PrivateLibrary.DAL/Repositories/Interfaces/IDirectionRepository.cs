using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateLibrary.DAL.Models.Book;

namespace PrivateLibrary.DAL.Repositories.Interfaces
{
    public interface IDirectionRepository : IEntityBaseRepository<int, Direction>
    {
    }
}
