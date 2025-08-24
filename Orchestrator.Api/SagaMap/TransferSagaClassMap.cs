using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Orchestrator.Api.StateMachineInstances;

namespace Orchestrator.Api.SagaMap
{
    public class TransferSagaClassMap : SagaClassMap<TransferStateMachineInstance>
    {
        public Type SagaType { get; set; }
        protected override void Configure(EntityTypeBuilder<TransferStateMachineInstance> entity, ModelBuilder model)
        {
            entity.Property(x => x.CorrelationId);
            entity.Property(x => x.TransactionId);

            // If using Optimistic concurrency, otherwise remove this property
            entity.Property(x => x.State);
            entity.Property(x => x.LastUpdated).IsConcurrencyToken().HasDefaultValueSql("CURRENT_TIMESTAMP");


            //model.UseSqlServer();
        }
    }
}
