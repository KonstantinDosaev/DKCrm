﻿using DKCrm.Shared.Models.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models
{
    public class FilterProductTuple
    {
        public Guid? CategoryId { get; set; } 
        public List<Guid>? BrandIdList { get; set; }
        public List<Guid>? ProductOptions { get; set; }
    }
}