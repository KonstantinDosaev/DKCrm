using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Iterfaces
{
    internal interface IIdentifiable
    {
        Guid Id { get; }
    }
}
