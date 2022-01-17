using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PrivateLibrary.DAL.Contexts;
using PrivateLibrary.DAL.Models.Book;
using PrivateLibrary.DAL.Repositories.Interfaces;

namespace PrivateLibrary.DAL.Repositories.Realizations
{
    public class CustomerBookRepository : EntityBaseRepository<Guid, LibraryCustomer, LibraryDbContext>, ICustomerBookRepository
    {
        public CustomerBookRepository(LibraryDbContext dbContext) : base(dbContext)
        {
        }

        public override async Task<LibraryCustomer?> GetByIdAsync(Guid id)
        {
            return await DbSet.Where(x => x.Id == id)
                .Include(x => x.Books).FirstOrDefaultAsync();
        }

        public async Task AddBookForCustomer(LibraryCustomer customer, Book book)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(book, nameof(book));

            customer.Books!.Add(book);
            await DbContext.SaveChangesAsync();
        }

        public async Task RemoveBookForCustomer(LibraryCustomer customer, Book book)
        {
            ArgumentNullException.ThrowIfNull(customer, nameof(customer));
            ArgumentNullException.ThrowIfNull(book, nameof(book));

            customer.Books!.Remove(book);
            await DbContext.SaveChangesAsync();
        }
    }
}
