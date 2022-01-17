using PrivateLibrary.Models;
using PrivateLibrary.Models.Books;

namespace PrivateLibrary.ViewModels.Books
{
    public class BookListViewModel
    {
        public SearchResult<ShortBookModel>? SearchResultModel { get; set; }

        public BookFilter? BookFilterModel { get; set; }
    }
}
