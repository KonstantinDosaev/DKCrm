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
        public DbSet<GrandCategory> GrandCategories { get; set; } = null!;
        public DbSet<ProductOption> ProductOptions { get; set; } = null!;
        public DbSet<CategoryOption> CategoryOptions { get; set; } = null!;

    }
}
