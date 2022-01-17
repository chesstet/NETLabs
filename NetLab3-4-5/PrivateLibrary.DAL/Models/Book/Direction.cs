namespace PrivateLibrary.DAL.Models.Book
{
    public class Direction
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public ICollection<Book>? Books { get; set; }
    }
}
