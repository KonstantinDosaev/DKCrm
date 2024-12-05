using DKCrm.Server.Services;
using DKCrm.Shared.Models;
using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.OrderModels;
using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    optionsBuilder
        //        .UseLazyLoadingProxies()        // подключение lazy loading
        //        .UseNpgsql("Data Source=helloapp.db");
        //}
        public DbSet<Product> Products { get; set; } = null!;
        public DbSet<Brand> Brands { get; set; } = null!;
        public DbSet<Category> Categories { get; set; } = null!;

        public DbSet<ProductOption> ProductOptions { get; set; } = null!;
        public DbSet<CategoryOption> CategoryOptions { get; set; } = null!;

        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<CompanyType> CompanyTypes { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<FnsRequest> FnsRequests { get; set; } = null!;
        public DbSet<TagsCompany> TagsCompanies { get; set; } = null!;
        public DbSet<BankDetails> BankDetails { get; set; } = null!;
        public DbSet<Storage> Storages { get; set; } = null!;
        public DbSet<ProductsInStorage> ProductsInStorages { get; set; } = null!;

        public DbSet<CommentOrder> CommentOrders { get; set; } = null!;
        public DbSet<ExportedOrder> ExportedOrders { get; set; } = null!;
        public DbSet<ExportedOrderStatus> ExportedOrderStatus { get; set; } = null!;
        public DbSet<ExportedProduct> ExportedProducts { get; set; } = null!;
        public DbSet<ImportedOrder> ImportedOrders { get; set; } = null!;
        public DbSet<ImportedOrderStatus> ImportedOrderStatus { get; set; } = null!;
        public DbSet<ImportedProduct> ImportedProducts { get; set; } = null!;
        public DbSet<PurchaseAtExport> PurchaseAtExports { get; set; } = null!;
        public DbSet<PurchaseAtStorage> PurchaseAtStorages { get; set; } = null!;
        public DbSet<SoldFromStorage> SoldFromStorages { get; set; } = null!;
        public DbSet<InternalCompanyData> InternalCompanyData { get; set; } = null!;

        public DbSet<ApplicationOrderingProducts> ApplicationOrderingProducts { get; set; } = null!;
        public DbSet<ApplicationOrderingProductsProduct> ApplicationOrderingProductsProducts { get; set; } = null!;
        public DbSet<CurrencyDictionary> CurrencyDictionaries { get; set; } = null!;
        public DbSet<InfoSetFromDocumentToOrder> DocumentsToOrder { get; set; } = null!;
        public DbSet<InfoSetToImage> InfoSetsToImages { get; set; } = null!;
        public DbSet<CompanyComment> CompanyComments { get; set; } = null!;
        public DbSet<AccessRestriction> AccessRestrictions { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Product>()
                .HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<Storage>()
                .HasQueryFilter(x => x.IsDeleted == false);
            builder.Entity<ApplicationOrderingProducts>()
                .HasQueryFilter(x => x.IsDeleted == false);

            builder.Entity<Category>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

            builder.Entity<Product>()
                .HasOne(u => u.Category)
                .WithMany(c => c.Products).HasForeignKey(f=>f.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);
           
            builder.Entity<Company>(entity =>
            {
                entity.HasOne(d => d.FnsRequest)
                    .WithOne(p => p.Company)
                    .HasForeignKey<FnsRequest>(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasMany(d => d.Comments)
                    .WithOne(p => p.Company)
                    .HasForeignKey(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder
                .Entity<Storage>()
                .HasMany(c => c.Products)
                .WithMany(s => s.Storage)
                .UsingEntity<ProductsInStorage>(
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(t => t.ProductsInStorage)
                        .HasForeignKey(pt => pt.ProductId),
                    j => j
                        .HasOne(pt => pt.Storage)
                        .WithMany(p => p.ProductsInStorage)
                        .HasForeignKey(pt => pt.StorageId),
                    j =>
            {
                j.Property(pt => pt.Quantity).HasDefaultValue(0);
                j.HasKey(t => new { t.StorageId, t.ProductId });
                j.ToTable("ProductsInStorages");
            });

           



            builder.Entity<ExportedOrder>()
                .HasOne(m => m.OurCompany)
                .WithMany(t => t.ExportedOrdersOurCompany)
                .HasForeignKey(m => m.OurCompanyId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ExportedOrder>()
                .HasOne(m => m.CompanyBuyer)
                .WithMany(t => t.ExportedOrdersBuyerCompany)
                .HasForeignKey(m => m.CompanyBuyerId).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ExportedOrder>()
                .HasOne(m => m.OurEmployee)
                .WithMany(t => t.ExportedOrdersOur)
                .HasForeignKey(m => m.OurEmployeeId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ExportedOrder>()
                .HasOne(m => m.EmployeeBuyer)
                .WithMany(t => t.ExportedOrdersBuyer)
                .HasForeignKey(m => m.EmployeeBuyerId).OnDelete(DeleteBehavior.SetNull);


            builder.Entity<ImportedOrder>()
                .HasOne(m => m.OurCompany)
                .WithMany(t => t.ImportedOrdersOurCompany)
                .HasForeignKey(m => m.OurCompanyId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ImportedOrder>()
                .HasOne(m => m.SellersCompany)
                .WithMany(t => t.ImportedOrdersSellersCompany)
                .HasForeignKey(m => m.SellersCompanyId).OnDelete(DeleteBehavior.SetNull);

            builder.Entity<ImportedOrder>()
                .HasOne(m => m.OurEmployee)
                .WithMany(t => t.ImportedOrdersOur)
                .HasForeignKey(m => m.OurEmployeeId).OnDelete(DeleteBehavior.SetNull);
            builder.Entity<ImportedOrder>()
                .HasOne(m => m.EmployeeSeller)
                .WithMany(t => t.ImportedOrdersSellers)
                .HasForeignKey(m => m.EmployeeSellerId).OnDelete(DeleteBehavior.SetNull);

         

            builder
                .Entity<ImportedProduct>()
                .HasMany(c => c.StorageList)
                .WithMany(s => s.ImportedProducts)
                .UsingEntity<PurchaseAtStorage>(
                    j => j
                        .HasOne(pt => pt.Storage)
                        .WithMany(t => t.PurchaseAtStorageList)
                        .HasForeignKey(pt => pt.StorageId),
                    j => j
                        .HasOne(pt => pt.ImportedProduct)
                        .WithMany(p => p.PurchaseAtStorageList)
                        .HasForeignKey(pt => pt.ImportedProductId), j =>
                    {
                        j.Property(pt => pt.Quantity).HasDefaultValue(0);
                        j.HasKey(t => new { t.StorageId, t.ImportedProductId });
                        j.ToTable("PurchaseAtStorages");
                    });


            builder
                .Entity<ExportedProduct>()
                .HasMany(c => c.StorageList)
                .WithMany(s => s.ExportedProducts)
                .UsingEntity<SoldFromStorage>(
                    j => j
                        .HasOne(pt => pt.Storage)
                        .WithMany(t => t.SoldFromStorageList)
                        .HasForeignKey(pt => pt.StorageId),
                    j => j
                        .HasOne(pt => pt.ExportedProduct)
                        .WithMany(p => p.SoldFromStorage)
                        .HasForeignKey(pt => pt.ExportedProductId), 
                    j =>
                    {
                        j.Property(pt => pt.Quantity).HasDefaultValue(0);
                        j.HasKey(t => new { t.StorageId, t.ExportedProductId });
                        j.ToTable("SoldFromStorages");
                    });

            builder
                .Entity<ExportedProduct>()
                .HasMany(c => c.ImportedProducts)
                .WithMany(s => s.ExportedProducts)
                .UsingEntity<PurchaseAtExport>(
                    j => j
                        .HasOne(pt => pt.ImportedProduct)
                        .WithMany(t => t.PurchaseAtExportList)
                        .HasForeignKey(pt => pt.ImportedProductId),
                    j => j
                        .HasOne(pt => pt.ExportedProduct)
                        .WithMany(p => p.PurchaseAtExports)
                        .HasForeignKey(pt => pt.ExportedProductId),
            j =>
            {
                j.Property(pt => pt.Quantity).HasDefaultValue(0);
                j.HasKey(t => new { t.ExportedProductId, t.ImportedProductId });
                j.ToTable("PurchaseAtExports");
            });

            builder
                .Entity<ApplicationOrderingProducts>()
                .HasMany(c => c.ProductList)
                .WithMany(s => s.ApplicationOrderingList)
                .UsingEntity<ApplicationOrderingProductsProduct>(
                    j => j
                        .HasOne(pt => pt.Product)
                        .WithMany(t => t.ApplicationOrderingProductProduct)
                        .HasForeignKey(pt => pt.ProductId),
                    j => j
                        .HasOne(pt => pt.ApplicationOrdering)
                        .WithMany(p => p.ApplicationOrderingProductProduct)
                        .HasForeignKey(pt => pt.ApplicationOrderingId),
                    j =>
                    {
                        j.Property(pt => pt.Quantity).HasDefaultValue(0);
                        j.HasKey(t => new { t.ApplicationOrderingId, t.ProductId });
                        j.ToTable("ApplicationOrderingProductsProducts");
                    });

            builder
                .Entity<ExportedOrder>()
                .HasMany(c => c.ExportedOrderStatus)
                .WithMany(s => s.ExportedOrders)
                .UsingEntity<ExportedOrderStatusExportedOrder>(
                    j => j
                        .HasOne(pt => pt.ExportedOrderStatus)
                        .WithMany(t => t.ExportedOrderStatusExported)
                        .HasForeignKey(pt => pt.ExportedOrderStatusId),
                    j => j
                        .HasOne(pt => pt.ExportedOrder)
                        .WithMany(p => p.ExportedOrderStatusExported)
                        .HasForeignKey(pt => pt.ExportedOrderId), j =>
                    {
                        j.HasKey(t => new { t.ExportedOrderId, t.ExportedOrderStatusId });
                        j.ToTable("ExportedOrderStatusExportedOrder");
                    });
            builder
                .Entity<ImportedOrder>()
                .HasMany(c => c.ImportedOrderStatus)
                .WithMany(s => s.ImportedOrders)
                .UsingEntity<ImportedOrderStatusImportedOrder>(
                    j => j
                        .HasOne(pt => pt.ImportedOrderStatus)
                        .WithMany(t => t.ImportedOrderStatusImportedOrders)
                        .HasForeignKey(pt => pt.ImportedOrderStatusId),
                    j => j
                        .HasOne(pt => pt.ImportedOrder)
                        .WithMany(p => p.ImportedOrderStatusImportedOrders)
                        .HasForeignKey(pt => pt.ImportedOrderId), j =>
                    {
                        j.HasKey(t => new { t.ImportedOrderId, t.ImportedOrderStatusId });
                        j.ToTable("ImportedOrderStatusImportedOrder");
                    });
            builder.Entity<ExportedOrderStatus>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });
            builder.Entity<ImportedOrderStatus>(entity =>
            {
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.SetNull);
            });

        }

    }
}
