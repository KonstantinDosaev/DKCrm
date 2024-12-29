using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class LogUsersVisitToCompanyComments
    {
        public Guid Id { get; set; }
        public DateTime DateTimeVisit { get; set; }
        [MaxLength(50)]
        public string UserId { get; set; } = null!;

        public Guid CompanyOwnerCommentsId { get; set; }
    }
}
