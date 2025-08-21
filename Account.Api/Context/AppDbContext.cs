using Account.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Account.Api.Context
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Accounts { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
        }

        
    }
}
