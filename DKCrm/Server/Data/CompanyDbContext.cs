using DKCrm.Shared.Models.CompanyModels;
using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Data
{
    public class CompanyDbContext : DbContext
    {
        public CompanyDbContext(DbContextOptions<CompanyDbContext> options) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<CompanyType> CompanyTypes { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        public DbSet<FnsRequest> FnsRequests { get; set; } = null!;
        public DbSet<TagsCompany> TagsCompanies { get; set; } = null!;
        public DbSet<BankDetails> BankDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Company>(entity =>
            {
                entity.HasOne(d => d.FnsRequest)
                    .WithOne(p => p.Company)
                    .HasForeignKey<FnsRequest>(d => d.CompanyId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            //builder.Entity<Product>()
            //    .HasOne(u => u.Category)
            //    .WithMany(c => c.Products).HasForeignKey(f => f.CategoryId)
            //    .OnDelete(DeleteBehavior.Cascade);
            ////builder.Entity<Category>()
            ////    .HasMany(u => u.Products)
            ////    .WithOne(c => c.Category)
            ////    .OnDelete(DeleteBehavior.SetNull);

        }
    }
}
