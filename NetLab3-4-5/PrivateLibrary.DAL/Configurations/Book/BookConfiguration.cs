using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace PrivateLibrary.DAL.Configurations.Book
{
    internal class BookConfiguration : IEntityTypeConfiguration<Models.Book.Book>
    {
        public void Configure(EntityTypeBuilder<Models.Book.Book> builder)
        {
            builder.HasKey(nameof(Models.Book.Book.Id));

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.AuthorId);

            builder.HasOne(x => x.Direction)
                .WithMany(x => x.Books)
                .HasForeignKey(x => x.DirectionId);

            builder.HasMany(x => x.Customers)
                .WithMany(x => x.Books);
        }
    }
}
