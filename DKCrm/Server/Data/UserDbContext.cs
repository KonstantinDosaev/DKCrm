using DKCrm.Server.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DKCrm.Server.Data
{
    public class UserDbContext: IdentityDbContext<ApplicationUser>
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }
    }
}
