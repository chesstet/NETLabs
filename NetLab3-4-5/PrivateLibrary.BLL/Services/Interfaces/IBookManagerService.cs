using PrivateLibrary.BLL.Dtos.Book;
using PrivateLibrary.BLL.Dtos.Searching;

namespace PrivateLibrary.BLL.Services.Interfaces
{
    public interface IBookManagerService : ICrudService<Guid, BookDto?>
    {
        public Task<IEnumerable<ShortBookDto>?> GetByAuthorIdAsync(Guid id);
        public Task<IEnumerable<ShortBookDto>?> GetByLibraryCustomerIdAsync(Guid id);

        public Task<SearchResultDto<ShortBookDto>> GetByFilterAsync(ShortBookFilterDto? filterDto);
    }
}
