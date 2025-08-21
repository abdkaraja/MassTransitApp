using Microsoft.EntityFrameworkCore;
using Orchestrator.Api.StateMachineInstances;
using System.Data.Common;

namespace Orchestrator.Api.Contexts
{
    public class OrchestratorContext : DbContext
    {
        public DbSet<TransferStateMachineInstance> TransferStateMachineInstances { get; set; }

        public OrchestratorContext(DbContextOptions<OrchestratorContext> options)
                : base(options)
        {
        }
    }
}
