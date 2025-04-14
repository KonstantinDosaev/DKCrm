using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DKCrm.Shared.Models.OfferModels
{
    public class FilterOfferTuple
    {

        public DateTime? CreateDateFirst { get; set; }
        public DateTime? CreateDateLast { get; set; }
        public DateTime? UpdateDateFirst { get; set; }
        public DateTime? UpdateDateLast { get; set; }
        public Guid? CurrentCompanyId { get; set; }
        public Guid? ProductId { get; set; }
        public bool LoadOnlyNotOver { get; set; }
        
    }
}
