using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class Employee : IIdentifiable
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string? LastName { get; set; }
        public string? Position { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set;}
        public string? Description { get; set; }

        public virtual Company? Company { get; set; }
        public Guid CompanyId { get; set; }
    }
}
