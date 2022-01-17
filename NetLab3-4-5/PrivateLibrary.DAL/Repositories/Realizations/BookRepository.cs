using Microsoft.EntityFrameworkCore;
using PrivateLibrary.DAL.Contexts;
using PrivateLibrary.DAL.Models.Book;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.DAL.Repositories.Realizations
{
    public class BookRepository : EntityBaseRepository<Guid, Book, LibraryDbContext>, IBookRepository
    {
        public BookRepository(LibraryDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<Book?> GetByIdAsync(Guid id)
        {
            return await DbSet.Where(x => x.Id == id)
                .Include(x => x.Author)
                .Include(x => x.Direction).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid id)
        {
            return await DbSet.Where(x => x.AuthorId == id).ToListAsync();
        }

        public async Task<IEnumerable<Book>?> GetByLibraryCustomerIdAsync(Guid id)
        {
            return await DbContext.LibraryCustomers!.Where(x => x.Id == id).Select(x => x.Books).FirstOrDefaultAsync();
        }
    }
}
