using MassTransitApp.Saga;
using Microsoft.EntityFrameworkCore;

namespace MassTransitApp
{

    public class AppDbContext : DbContext
    {
        public DbSet<Subscriper> Subscripers { get; set; }
        public DbSet<NewsletterOnboardingSagaData> NewsletterOnboardingSagaData { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsletterOnboardingSagaData>().HasKey(s => s.CorrelationId);
        }

    }

}
