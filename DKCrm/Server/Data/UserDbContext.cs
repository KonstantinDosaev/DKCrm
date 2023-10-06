using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Chat;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DKCrm.Server.Data
{
    public class UserDbContext: IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<Address> Addresses { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ChatMessage>(entity =>
            {
                entity.HasOne(d => d.FromUser)
                    .WithMany(p => p.ChatMessagesFromUsers)
                    .HasForeignKey(d => d.FromUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
                entity.HasOne(d => d.ToUser)
                    .WithMany(p => p.ChatMessagesToUsers)
                    .HasForeignKey(d => d.ToUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull);
            });
            //builder.Entity<ApplicationUser>(entity =>
            //{
            //    entity.HasOne(d => d.Address)
            //        .WithMany(p => p)
            //        .HasForeignKey(d => d.FromUserId)
            //        .OnDelete(DeleteBehavior.ClientSetNull);
            //    entity.HasOne(d => d.ToUser)
            //        .WithMany(p => p.ChatMessagesToUsers)
            //        .HasForeignKey(d => d.ToUserId)
            //        .OnDelete(DeleteBehavior.ClientSetNull);
            //});
        }
    }
}
