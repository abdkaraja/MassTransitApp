using MassTransit;
using RoutingSlipApi.Models.Arguments;
using RoutingSlipApi.Models.Events;
using RoutingSlipApi.Models.Logs;

namespace RoutingSlipApi.Activities
{
    public class ProcessPaymentActivity : IActivity<ProcessPaymentArguments, PaymentProcessedLog>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<ProcessPaymentArguments> context)
        {
            var args = context.Arguments;

            // Simulate payment processing
            await Task.Delay(1000);

            // Simulate payment gateway call
            var transactionId = Guid.NewGuid().ToString();

            // Add transaction ID to routing slip variables for next activity
            //await context.SetVariable("TransactionId", transactionId);


            // Publish domain event
            await context.Publish(new PaymentProcessed(args.OrderId, transactionId, args.Amount));

            return context.Completed();
        }

        public async Task<CompensationResult> Compensate(CompensateContext<PaymentProcessedLog> context)
        {
            var log = context.Log;

            
            // Simulate refund/reversal
            await Task.Delay(500);

            return context.Compensated();
        }
    }
}
