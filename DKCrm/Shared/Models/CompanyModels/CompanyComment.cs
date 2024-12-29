using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class CompanyComment
    {
        public Guid Id { get; set; }
        public bool IsWarningComment { get; set; }
        [MaxLength(4000)]
        public string Value { get; set; } = null!;
        [MaxLength(50)]
        public string FromUserId { get; set; } = null!;
        public DateTime DateTimeCreated { get; set; }
        public DateTime DateTimeUpdate { get; set; }
        public virtual Company? Company { get; set; }
        public Guid CompanyId { get; set; }
    }
}
