namespace PrivateLibrary.WebApi.Models.Books
{
    public class LibraryCustomerModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public IList<ShortBookModel>? Books { get; set; } = new List<ShortBookModel>();
    }
}
