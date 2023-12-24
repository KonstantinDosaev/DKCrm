using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class SortPagedResponse <T>
    {
        public IEnumerable<T>? Items { get; set; }
        public int TotalItems { get; set; }
    }
}
