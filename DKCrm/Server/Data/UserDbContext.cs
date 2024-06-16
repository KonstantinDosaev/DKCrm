using DKCrm.Shared.Models;
using DKCrm.Shared.Models.Chat;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Data
{
    public class UserDbContext: IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
        public DbSet<ChatMessage> ChatMessages { get; set; } = null!; 
        public DbSet<ChatGroup> ChatGroups { get; set; } = null!;
        public DbSet<LogUsersVisitToChat> LogUsersVisitToChats { get; set; } = null!;
        public DbSet<Address> Addresses { get; set; } = null!;
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().HasQueryFilter(x => x.IsDeleted == false);

           
            builder
                .Entity<ChatGroup>()
                .HasMany(c => c.ApplicationUsers)
                .WithMany(s => s.ChatGroups)
                .UsingEntity<LogUsersVisitToChat>(
                    j => j
                        .HasOne(pt => pt.ApplicationUser)
                        .WithMany(t => t.LogUsersVisitToChatList)
                        .HasForeignKey(pt => pt.ApplicationUserId),
                    j => j
                        .HasOne(pt => pt.ChatGroup)
                        .WithMany(p => p.LogUsersVisitToChatList)
                        .HasForeignKey(pt => pt.ChatGroupId),
                    j =>
                    {
                        j.HasKey(t => new { t.ApplicationUserId, t.ChatGroupId });
                        j.ToTable("LogUsersVisitToChats");
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
