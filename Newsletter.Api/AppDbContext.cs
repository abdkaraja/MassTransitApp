using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Entities;

namespace Newsletter.Api
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
        }

        public DbSet<Article> Articles { get; set; }
    }
}
