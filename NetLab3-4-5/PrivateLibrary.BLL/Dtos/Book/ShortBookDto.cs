using PrivateLibrary.BLL.Infrastructure.Enums.Filters.Books;

namespace PrivateLibrary.BLL.Dtos.Book
{
    public record ShortBookDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? AuthorName { get; set; }
        public string? DirectionName { get; set; }
    }
}
