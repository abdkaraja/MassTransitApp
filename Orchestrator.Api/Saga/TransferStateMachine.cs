using MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using Orchestrator.Api.StateMachineInstances;

namespace Orchestrator.Api.Saga
{
    public class TransferStateMachine : MassTransitStateMachine<TransferStateMachineInstance>
    {
        public TransferStateMachine()
        {
            InstanceState(x => x.State);

            Event(() => StartTransactionEvent, x => x.CorrelateById(context => context.Message.TransactionId));
            Event(() => OnCommissionCalculateEvent, x => x.CorrelateById(context => context.Message.TransactionId));
            Event(() => UpdateAmountEvent, x => x.CorrelateById(context => context.Message.TransactionId));
            Event(() => UpdateAmountCompensateEvent, x => x.CorrelateById(context => context.Message.TransactionId));
            Event(() => AddTransactionLogEvent, x => x.CorrelateById(context => context.Message.TransactionId));

            Initially(
                When(StartTransactionEvent).
                Then(context =>
                {
                    context.Saga.TransactionId = context.Message.TransactionId;
                }).
                TransitionTo(Started));

            During(Started,
            When(OnCommissionCalculateEvent)
                .TransitionTo(CommissionCalculated));

            During(CommissionCalculated,
            When(UpdateAmountEvent)
                .TransitionTo(UpdateAmountExecuted));

            During(CommissionCalculated,
            When(UpdateAmountCompensateEvent)
                .TransitionTo(UpdateAmountCompensated));

            During(UpdateAmountExecuted,
            When(AddTransactionLogEvent)
                .TransitionTo(TransactionLogCreated));

            
        }

        // Events
        public Event<StartTransactionEvent> StartTransactionEvent { get; private set; }
        public Event<OnCommissionCalculateEvent> OnCommissionCalculateEvent { get; private set; }
        public Event<UpdateAmountEvent> UpdateAmountEvent { get; private set; }
        public Event<UpdateAmountCompensateEvent> UpdateAmountCompensateEvent { get; private set; }
        public Event<AddTransactionLogEvent> AddTransactionLogEvent { get; private set; }

        //States
        public State Started { get; private set; }
        public State CommissionCalculated { get; private set; }
        public State UpdateAmountExecuted { get; private set; }
        public State UpdateAmountCompensated { get; private set; }
        public State TransactionLogCreated { get; private set; }
    }

    public class StartTransactionEvent
    {
        public Guid TransactionId { get; set; }
    }

    public class OnCommissionCalculateEvent
    {
        public Guid TransactionId { get; set; }
    }

    public class UpdateAmountEvent
    {
        public Guid TransactionId { get; set; }
    }

    public class UpdateAmountCompensateEvent
    {
        public Guid TransactionId { get; set; }
    }

    public class AddTransactionLogEvent
    {
        public Guid TransactionId { get; set; }
    }
}
