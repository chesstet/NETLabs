using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateLibrary.BLL.Dtos.Book
{
    public record LibraryCustomerDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }

        public List<ShortBookDto>? Books { get; set; }
    }
}
