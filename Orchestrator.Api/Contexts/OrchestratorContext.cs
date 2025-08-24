using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Api.SagaMap;
using Orchestrator.Api.StateMachineInstances;

namespace Orchestrator.Api.Contexts
{
    public class OrchestratorContext : SagaDbContext
    {
        public DbSet<TransferStateMachineInstance> TransferStateMachineInstances { get; set; }

        protected override IEnumerable<ISagaClassMap> Configurations
        {
            get
            {
                yield return new TransferSagaClassMap();
            }
        }

        public OrchestratorContext(DbContextOptions<OrchestratorContext> options)
                : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.AddInboxStateEntity();
            modelBuilder.AddOutboxMessageEntity();
            modelBuilder.AddOutboxStateEntity();
        }
    }
}
