using DKCrm.Shared.Models.Chat;
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
        //public DbSet<GrandCategory> GrandCategories { get; set; } = null!;
        public DbSet<ProductOption> ProductOptions { get; set; } = null!;
        public DbSet<CategoryOption> CategoryOptions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
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
            //builder.Entity<Category>()
            //    .HasMany(u => u.Products)
            //    .WithOne(c => c.Category)
            //    .OnDelete(DeleteBehavior.SetNull);

        }

    }
}
