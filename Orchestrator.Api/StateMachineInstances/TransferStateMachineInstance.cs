using MassTransit;

namespace Orchestrator.Api.StateMachineInstances
{
    public class TransferStateMachineInstance : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public Guid TransactionId { get; set; }
        public string State { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
