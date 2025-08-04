using MassTransit;
using MassTransitApp.Models;

namespace MassTransitApp.Cunsumers
{
    public class SubscribeToNewsletterConsumer(AppDbContext dbContext) 
        : IConsumer<SubscribeToNewsletter>
    {
        public async Task Consume(ConsumeContext<SubscribeToNewsletter> context)
        {
            var subscriber = dbContext.Subscripers.Add(new Subscriper
            {
                Email = context.Message.Email,
                Id = Guid.NewGuid(),
                SubscripedDate = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();

            await context.Publish(new SubscriberCreated
            {
                SubscriberId = subscriber.Entity.Id,
                Email = context.Message.Email
            });
        }
    }
}
