using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Contracts;
using Newsletter.Api.Shared;
using Newsletter.Shared;

namespace Newsletter.Api.Features.Articles;

public static class GetArticle
{
    public class Query : IRequest<Result<Contracts.ArticleResponse>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<Contracts.ArticleResponse>>
    {
        private readonly AppDbContext _dbContext;
        private readonly IPublishEndpoint _publishEndpoint;
        public Handler(AppDbContext dbContext, IPublishEndpoint publishEndpoint)
        {
            _dbContext = dbContext;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<Result<Contracts.ArticleResponse>> Handle(Query request, CancellationToken cancellationToken)
        {
            var articleResponse = await _dbContext
                .Articles
                .AsNoTracking()
                .Where(article => article.Id == request.Id)
                .Select(article => new Contracts.ArticleResponse
                {
                    Id = article.Id,
                    Title = article.Title,
                    Content = article.Content,
                    Tags = article.Tags,
                    CreatedOnUtc = article.CreatedOnUtc,
                    PublishedOnUtc = article.PublishedOnUtc
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (articleResponse is null)
            {
                return Result.Failure<Contracts.ArticleResponse>(new Error(
                    "GetArticle.Null",
                    "The article with the specified ID was not found"));
            }

            await _publishEndpoint.Publish(
                new ArticleViewedEvent
                {
                    Id = articleResponse.Id,
                    ViewedOnUtc = DateTime.UtcNow
                },
                cancellationToken);

            return articleResponse;
        }
    }
}

