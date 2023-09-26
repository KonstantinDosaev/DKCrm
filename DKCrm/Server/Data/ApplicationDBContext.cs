using DKCrm.Shared.Models.Products;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; } = null!;
    }
}
