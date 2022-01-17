using PrivateLibrary.DAL.Models.Book;

namespace PrivateLibrary.DAL.Repositories.Interfaces
{
    public interface IBookRepository : IEntityBaseRepository<Guid, Book>
    {
        public Task<IEnumerable<Book>> GetByAuthorIdAsync(Guid id);

        public Task<IEnumerable<Book>?> GetByLibraryCustomerIdAsync(Guid id);
    }
}
