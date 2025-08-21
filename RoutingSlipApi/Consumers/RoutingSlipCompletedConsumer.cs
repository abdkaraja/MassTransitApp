using MassTransit;
using MassTransit.Courier.Contracts;

namespace RoutingSlipApi.Consumers
{
    public class RoutingSlipCompletedConsumer : IConsumer<RoutingSlipCompleted>
    {
        private readonly ILogger<RoutingSlipCompletedConsumer> _logger;

        public RoutingSlipCompletedConsumer(ILogger<RoutingSlipCompletedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<RoutingSlipCompleted> context)
        {
            var trackingNumber = context.Message.TrackingNumber;
            var orderIdVariable = context.Message.Variables["OrderId"];

            _logger.LogInformation("Routing slip {TrackingNumber} completed successfully for Order {OrderId}",
                trackingNumber, orderIdVariable);

            return Task.CompletedTask;
        }
    }
}
