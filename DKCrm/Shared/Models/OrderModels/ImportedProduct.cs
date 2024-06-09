using DKCrm.Shared.Iterfaces;
using DKCrm.Shared.Models.Products;

namespace DKCrm.Shared.Models.OrderModels
{
    public class ImportedProduct: IIdentifiable, ISoftDelete
    {
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public decimal? PriceLocal { get; set; }
        public string? TransactionCurrency { get; set; }
        public decimal? PriceInTransactionCurrency { get; set; }
        public string? SupplierCurrency { get; set; }
        public decimal? PriceInSupplierCurrency { get; set; }
        public string? Description { get; set; }
        public DateTime? DateTimeConversionCurrency { get; set; }
        public virtual ICollection<ExportedProduct>? ExportedProducts { get; set; }
        public virtual ICollection<PurchaseAtStorage>? PurchaseAtStorageList { get; set; }
        public virtual ICollection<PurchaseAtExport>? PurchaseAtExportList { get; set; }
        public virtual ICollection<Storage>? StorageList { get; set; }

        public virtual Product? Product { get; set; }
        public Guid? ProductId { get; set; }
        public virtual ImportedOrder? ImportedOrder { get; set; }
        public Guid? ImportedOrderId { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFullDeleted { get; set; }
        public DateTime? DateTimeUpdate { get; set; }
        public string? UpdatedUser { get; set; }
    }
}
