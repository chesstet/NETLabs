using PrivateLibrary.BLL.Dtos.Searching;
using PrivateLibrary.BLL.Infrastructure.Enums.Filters.Books;

namespace PrivateLibrary.BLL.Dtos.Book
{
    public record ShortBookFilterDto : OffsetFilter
    {
        public List<Guid> AuthorIds { get; init; } = new();

        public string AuthorFirstName { get; init; } = string.Empty;

        public string AuthorLastName { get; init; } = string.Empty;

        public string BookName { get; init; } = string.Empty;

        public string DirectionName { get; init; } = string.Empty;

        public string OrderByField { get; set; } = nameof(OrderBy.AuthorBookAsc);

        public short BookYear { get; init; } = 0;
    }
}
