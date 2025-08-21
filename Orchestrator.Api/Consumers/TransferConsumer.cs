using MassTransit;
using MassTransit.Courier.Contracts;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Api.Activities;
using Orchestrator.Api.Endpoints;
using SharedProj.Consumers;

namespace Account.Api.Consumers
{
    public class TransferConsumer : IConsumer<TransferData>
    {
        private IBus _bus;

        public TransferConsumer(IBus bus)
        {
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<TransferData> context)
        {
            var data = context.Message;
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            var transactionId = Guid.NewGuid();

            builder.AddActivity("CalculateCommissionActivity",
                new Uri("queue:calculate-commission_execute"),
                new CalculateCommissionArguments
                {
                    TransactionId = transactionId,
                    Amount = context.Message.Amount
                });

            builder.AddActivity("UpdateAmountActivity",
                new Uri("queue:update-amount_execute"),
                new UpdateAmountArguments
                {
                    TransactionId = transactionId,
                    Amount = context.Message.Amount,
                    From = context.Message.From,
                    To = context.Message.To
                });

            builder.AddActivity("AddTransactionLogActivity",
                new Uri("queue:add-transaction-log_execute"),
                new AddTransactionLogArguments
                {
                    TransactionId = transactionId,
                    Amount = context.Message.Amount,
                    From = context.Message.From,
                    To = context.Message.To
                });
            await builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Completed,x=>x.Send(new TransferData
            {
                TransactionId = context.Message.TransactionId,
                Amount = context.Message.Amount,
                From = context.Message.From,
                To = context.Message.To,
                IsTransactionDone = true
            }, (sendContext) =>
            {
                sendContext.RequestId = context.RequestId; sendContext.InitiatorId = context.InitiatorId;
                sendContext.ConversationId = context.ConversationId;
            }, new CancellationToken()));

            await builder.AddSubscription(context.SourceAddress, RoutingSlipEvents.Faulted, RoutingSlipEventContents.All, x => x.Send(new TransferData
            {
                IsTransactionDone = false,
                Message = context.Message.Message
            }, (sendContext) =>
            {
                sendContext.RequestId = context.RequestId; sendContext.InitiatorId = context.InitiatorId;
                sendContext.ConversationId = context.ConversationId;
            }, new CancellationToken()));

            var routingSlip = builder.Build();
            await _bus.Execute(routingSlip);
        }
    }
}
