using ConsumerApp.Models;
using MassTransit;

namespace ConsumerApp.Consumers
{
    /// <summary>
    /// A class that consumes one or more messages types, one for each implementation of IConsumer<TMessage>
    /// </summary>
    public class SubmitOrderConsumer : IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            await context.Publish<OrderSubmitted>(new
            {
                context.Message.OrderId
            });
        }
    }
}
