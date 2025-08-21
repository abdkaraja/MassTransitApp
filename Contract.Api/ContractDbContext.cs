using Contract.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Contract.Api
{
    public class ContractDbContext: DbContext
    {
        public DbSet<Commission> Commissions { get; set; }
        public DbSet<TransactionLog> TransactionLogs { get; set; }

        public ContractDbContext(DbContextOptions<ContractDbContext> options)
                : base(options)
        {
        }
    }
}
