﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;

namespace DKCrm.Shared.Models.CompanyModels
{
    public class BankDetails : IIdentifiable
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? INN { get; set; }
        public string BankAccount { get; set; }
        public string BeneficiaryAccount { get; set; }
        public string? Description { get; set; }

        public virtual Company? Company { get; set; }
        public  Guid CompanyId { get; set; }

    }
}