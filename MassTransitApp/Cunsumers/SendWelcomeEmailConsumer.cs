using MassTransit;
using MassTransitApp.Models;

namespace MassTransitApp.Cunsumers
{
    public class SendWelcomeEmailConsumer : IConsumer<SendWelcomeEmail>
    {
        public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
        {
            // Send wellcome email code
            await context.Publish(new WelcomeEmailSent
            {
                Email = context.Message.Email,
                SubscriberId = context.Message.SubscriberId
            });
        }
    }
}
