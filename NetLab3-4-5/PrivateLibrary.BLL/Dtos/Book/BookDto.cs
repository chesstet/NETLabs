using PrivateLibrary.DAL.Models.Book;

namespace PrivateLibrary.BLL.Dtos.Book
{
    public record BookDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public AuthorDto? Author { get; set; }
        public short? Year { get; set; }
        public DirectionDto? Direction { get; set; }
    }
}
