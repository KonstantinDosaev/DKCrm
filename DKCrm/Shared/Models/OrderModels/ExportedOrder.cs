﻿using DKCrm.Shared.Models.CompanyModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ExportedOrder
    {
        public Guid Id { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string? Images { get; set; }
        public double? CurrencyPercent { get; set; }

        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<CommentOnExportedOrder>? Comments { get; set; }

        public virtual Company? OurCompany { get; set; }
        public Guid? OurCompanyId { get; set; }
        public virtual Company? CompanyBuyer { get; set; }
        public Guid? CompanyBuyerId { get; set; }

        public virtual Employee? OurEmployee { get; set; }
        public Guid? OurEmployeeId { get; set; }
        public virtual Employee? EmployeeBuyer { get; set; }
        public Guid? EmployeeBuyerId { get; set; }

        public virtual ExportedOrderStatus ? ExportedOrderStatus { get; set; }
        public Guid? ExportedOrderStatusId { get; set; }
      
    }
}