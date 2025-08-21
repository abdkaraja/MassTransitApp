using MassTransit;
using MassTransit.Transports;
using Microsoft.EntityFrameworkCore;
using Newsletter.Shared;

namespace Newsletter.Api.Consumers
{
    public class GetArticleDetailConsumer : IConsumer<ArticleRequest>
    {
        private readonly AppDbContext _appDbContext;

        public GetArticleDetailConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Consume(ConsumeContext<ArticleRequest> context)
        {
            var article = await _appDbContext.Articles
                .FirstOrDefaultAsync(x=>x.Id == context.Message.Id);

            await context.RespondAsync(new
            {
                CorrelationId = context.Message.Id,
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
            });
        }
    }
}
