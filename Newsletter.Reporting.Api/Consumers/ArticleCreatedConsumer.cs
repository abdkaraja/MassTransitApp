using MassTransit;
using Newsletter.Reporting.Api.Entities;
using Newsletter.Shared;

namespace Newsletter.Reporting.Api.Consumers
{
    public class ArticleCreatedConsumer : IConsumer<ArticleCreatedEvent>
    {
        private readonly AppDbContext _context;

        public ArticleCreatedConsumer(AppDbContext context)
        {
            _context = context;
        }
        public async Task Consume(ConsumeContext<ArticleCreatedEvent> context)
        {
            var article = new Article()
            {
                Id = context.Message.Id,
                CreatedOnUtc = context.Message.CreatedOnUtc
            };

            _context.Add(article);
            await _context.SaveChangesAsync();
        }
    }
}
