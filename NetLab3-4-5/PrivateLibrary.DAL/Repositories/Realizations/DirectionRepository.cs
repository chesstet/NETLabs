using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PrivateLibrary.DAL.Contexts;
using PrivateLibrary.DAL.Models.Book;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.DAL.Repositories.Realizations
{
    public class DirectionRepository : EntityBaseRepository<int, Direction, LibraryDbContext>, IDirectionRepository
    {
        public DirectionRepository(LibraryDbContext dbContext) : base(dbContext)
        {
        }
    }
}
