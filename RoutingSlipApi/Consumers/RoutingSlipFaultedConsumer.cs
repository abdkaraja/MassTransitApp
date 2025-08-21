using MassTransit;
using MassTransit.Courier.Contracts;

namespace RoutingSlipApi.Consumers
{
    public class RoutingSlipFaultedConsumer : IConsumer<RoutingSlipFaulted>
    {
        private readonly ILogger<RoutingSlipFaultedConsumer> _logger;

        public RoutingSlipFaultedConsumer(ILogger<RoutingSlipFaultedConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<RoutingSlipFaulted> context)
        {
            var trackingNumber = context.Message.TrackingNumber;
            var orderIdVariable = context.Message.Variables["OrderId"];

            _logger.LogError("Routing slip {TrackingNumber} faulted for Order {OrderId}. Reason: {Reason}",
                trackingNumber, orderIdVariable, context.Message.ActivityExceptions.FirstOrDefault()?.Name);

            return Task.CompletedTask;
        }
    }
}
