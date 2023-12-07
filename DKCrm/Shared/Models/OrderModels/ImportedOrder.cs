using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.CompanyModels;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ImportedOrder : IIdentifiable
    {
        public Guid Id { get; set; }
        public DateTime? DateTimeCreated { get; set; }
        public DateTime? DateTimeUpdated { get; set; }
        public string? Images { get; set; }
        public double? CurrencyPercent { get; set; }


        public virtual ICollection<ImportedProduct>? ImportedProducts { get; set; }
        public virtual ICollection<CommentOnImportedOrder>? Comments { get; set; }

        public virtual Company? OurCompany { get; set; }
        public Guid? OurCompanyId { get; set; }
        public virtual Company? SellersCompany { get; set; }
        public Guid? SellersCompanyId { get; set; }

        public virtual Employee? OurEmployee { get; set; }
        public Guid? OurEmployeeId { get; set; }
        public virtual Employee? EmployeeSeller{ get; set; }
        public Guid? EmployeeSellerId { get; set; }

        public virtual ImportedOrderStatus? ImportedOrderStatus { get; set; }
        public Guid? ImportedOrderStatusId { get; set; }
    }
}
