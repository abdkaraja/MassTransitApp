using MassTransit;
using Orchestrator.Api.StateMachineInstances;

namespace Orchestrator.Api.Saga
{
    public class TransferStateMachine : MassTransitStateMachine<TransferStateMachineInstance>
    {
        public TransferStateMachine()
        {
            InstanceState(x => x.State);

            Event(() => StartTransaction, x => x.CorrelateById(context => context.Message.TransactionId));
        }

        public Event<StartTransactionEvent> StartTransaction { get; private set; }
    }

    public class StartTransactionEvent
    {
        public Guid TransactionId { get; set; }
    }
}
