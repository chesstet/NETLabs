namespace PrivateLibrary.Models.Books
{
    public class ShortBookModel : BaseModel
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? AuthorName { get; set; }
        public string? OrderByField { get; set; }
        public string? DirectionName { get; set; }
    }
}
