using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateLibrary.DAL.Models.Book
{
    public class LibraryCustomer
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public virtual List<Book>? Books { get; set; }
    }
}
