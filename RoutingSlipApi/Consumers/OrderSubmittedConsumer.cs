using MassTransit;
using RoutingSlipApi.Models.Arguments;
using RoutingSlipApi.Models.Events;

namespace RoutingSlipApi.Consumers
{
    public class OrderSubmittedConsumer : IConsumer<OrderSubmitted>
    {
        private readonly ILogger<OrderSubmittedConsumer> _logger;

        public OrderSubmittedConsumer(ILogger<OrderSubmittedConsumer> logger)
        {
            _logger = logger;
        }

        public async Task Consume(ConsumeContext<OrderSubmitted> context)
        {
            var order = context.Message;

            _logger.LogInformation("Order submitted: {OrderId}, creating routing slip...", order.OrderId);

            // Create routing slip builder
            var builder = new RoutingSlipBuilder(NewId.NextGuid());

            // Add first activity: Process Payment
            builder.AddActivity("ProcessPayment",
                new Uri("queue:process-payment"),
                new ProcessPaymentArguments
                {
                    OrderId = order.OrderId,
                    Amount = order.Amount,
                    CustomerEmail = order.CustomerEmail
                });

            // Add second activity: Send Confirmation Email
            builder.AddActivity("SendConfirmationEmail",
                new Uri("queue:send-confirmation-email"),
                new SendConfirmationEmailArguments
                {
                    OrderId = order.OrderId,
                    CustomerEmail = order.CustomerEmail,
                    TransactionId = "" // Will be populated by previous activity
                });

            // Set initial routing slip variables
            builder.AddVariable("OrderId", order.OrderId);
            builder.AddVariable("CustomerEmail", order.CustomerEmail);

            // Execute the routing slip
            var routingSlip = builder.Build();
            await context.Execute(routingSlip);

            _logger.LogInformation("Routing slip created and executed for Order {OrderId}", order.OrderId);
        }
    }
}
