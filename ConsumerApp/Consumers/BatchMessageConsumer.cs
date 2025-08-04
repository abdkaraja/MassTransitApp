using ConsumerApp.Models;
using MassTransit;

namespace ConsumerApp.Consumers
{
    /// <summary>
    /// A class that consumes multiple messages in batches, by implementing IConsumer<Batch<TMessage>>
    /// </summary>
    public class BatchMessageConsumer :
    IConsumer<Batch<Message>>
    {
        public async Task Consume(ConsumeContext<Batch<Message>> context)
        {
            for (int i = 0; i < context.Message.Length; i++)
            {
                ConsumeContext<Message> message = context.Message[i];
            }
        }
    }
}
