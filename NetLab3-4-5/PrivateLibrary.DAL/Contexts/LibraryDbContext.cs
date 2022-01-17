using Microsoft.EntityFrameworkCore;
using PrivateLibrary.DAL.Configurations.Book;
using PrivateLibrary.DAL.Models.Book;

namespace PrivateLibrary.DAL.Contexts
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options)
        {
        }

        public DbSet<Author>? Authors { get; set; }
        public DbSet<Book>? Books { get; set; }
        public DbSet<Direction>? Directions { get; set; }
        public DbSet<LibraryCustomer>? LibraryCustomers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookConfiguration());
        }
    }
}
