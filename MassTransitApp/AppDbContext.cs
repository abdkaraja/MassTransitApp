using Microsoft.EntityFrameworkCore;

namespace MassTransitApp
{

    public class AppDbContext : DbContext
    {
        public DbSet<Subscriper> Subscripers { get; set; }


        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
        }

    }

}
