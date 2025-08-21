using MassTransit;
using RoutingSlipApi.Models.Arguments;
using RoutingSlipApi.Models.Events;
using RoutingSlipApi.Models.Logs;

namespace RoutingSlipApi.Activities
{
    public class SendConfirmationEmailActivity : IActivity<SendConfirmationEmailArguments, EmailSentLog>
    {
        public async Task<ExecutionResult> Execute(ExecuteContext<SendConfirmationEmailArguments> context)
        {
            var args = context.Arguments;

            // Simulate email service call
            await Task.Delay(500);

            // Simulate potential failure (uncomment to test compensation)
            // if (Random.Shared.NextDouble() < 0.3) // 30% failure rate
            // {
            //     throw new InvalidOperationException("Email service unavailable");
            // }

            // Publish domain event
            await context.Publish(new EmailSent(args.OrderId, args.CustomerEmail));

            return context.Completed();
        }

        public async Task<CompensationResult> Compensate(CompensateContext<EmailSentLog> context)
        {
            var log = context.Log;

            // In a real scenario, you might send a cancellation email
            // or update a database to mark the email as cancelled
            await Task.Delay(200);

            return context.Compensated();
        }
    }
}
