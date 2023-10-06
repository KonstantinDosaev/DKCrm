using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class RolesRequest
    {
        public string? UserName { get; set; }

        public string[]? RoleNames { get; set; }
    }
}
