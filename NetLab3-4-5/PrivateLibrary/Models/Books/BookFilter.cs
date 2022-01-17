namespace PrivateLibrary.Models.Books
{
    public class BookFilter : OffsetFilter
    {
        public List<Guid>? AuthorIds { get; set; }

        public string? AuthorFirstName { get; init; }

        public string? AuthorLastName { get; init; }

        public string? BookName { get; set; }

        public string? DirectionName { get; set; }

        public string? SearchText { get; set; }

        public short? BookYear { get; set; }

        public string? OrderByField { get; set; }
    }
}
