using MassTransit;
using MassTransitApp.Models;

namespace MassTransitApp.Cunsumers
{
    public class SendFollowUpEmailConsumer : IConsumer<SendFollowUpEmail>
    {
        public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
        {
            // Send wellcome email code
            await context.Publish(new FollowUpEmailSent
            {
                Email = context.Message.Email,
                SubscriberId = context.Message.SubscriberId
            });
        }
    }
}