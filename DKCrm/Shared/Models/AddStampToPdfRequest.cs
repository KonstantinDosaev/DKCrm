using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class AddStampToPdfRequest
    {
        public Guid InfoSetId { get; set; }
        public ICollection<StampPosition> StampPositionList { get; set; } = null!;
    }
}
