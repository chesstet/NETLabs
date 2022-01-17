using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrivateLibrary.BLL.Dtos.Searching
{
    public record OffsetFilter
    {
        public int Size { get; set; } = 6;
        public int From { get; set; } = 0;
    }
}
