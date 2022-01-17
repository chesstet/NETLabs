using System.ComponentModel.DataAnnotations;

namespace PrivateLibrary.DAL.Models.Book
{
    public class Book
    {
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public Guid? AuthorId { get; set; }
        public virtual Author? Author { get; set; }
        public short? Year { get; set; }
        public int? DirectionId { get; set; }
        public virtual Direction? Direction { get; set; }
        public virtual IList<LibraryCustomer>? Customers { get; set; }
    }
}
