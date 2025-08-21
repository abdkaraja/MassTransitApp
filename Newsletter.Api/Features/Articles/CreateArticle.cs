using FluentValidation;
using MassTransit;
using MediatR;
using Newsletter.Api.Entities;
using Newsletter.Api.Shared;
using Newsletter.Shared;

namespace Newsletter.Api.Features.Articles;

public static class CreateArticle
{
    public class Command : IRequest<Result<Guid>>
    {
        public string Title { get; set; } = string.Empty;

        public string Content { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;
    }

    internal sealed class Handler : IRequestHandler<Command, Result<Guid>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public Handler(AppDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<Guid>> Handle(Command request, CancellationToken cancellationToken)
        {
            var article = new Article
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Content = request.Content,
                Tags = request.Tags,
                CreatedOnUtc = DateTime.UtcNow
            };

            _dbContext.Add(article);

            await _dbContext.SaveChangesAsync(cancellationToken);

            await _publishEndpoint.Publish(new ArticleCreatedEvent
            {
                Id = article.Id,
                CreatedOnUtc = article.CreatedOnUtc,
            },cancellationToken);

            return article.Id;
        }
    }
}


