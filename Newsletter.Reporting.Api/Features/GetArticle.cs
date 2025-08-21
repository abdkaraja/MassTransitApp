using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Shared;
using Newsletter.Reporting.Api.Entities;
using Newsletter.Shared;

namespace Newsletter.Reporting.Api.Features.Articles;

public static class GetArticle
{
    public class Query : IRequest<Result<ArticleOutput>>
    {
        public Guid Id { get; set; }
    }

    internal sealed class Handler : IRequestHandler<Query, Result<ArticleOutput>>
    {
        private readonly AppDbContext _dbContext;
        public readonly IRequestClient<ArticleResponse> _requestClient;

        public Handler(AppDbContext dbContext, IRequestClient<ArticleResponse> requestClient)
        {
            _dbContext = dbContext;
            _requestClient = requestClient;
        }

        public async Task<Result<ArticleOutput>> Handle(Query request, CancellationToken cancellationToken)
        {
            var articleResponse = await _dbContext
                .Articles
                .AsNoTracking()
                .Where(article => article.Id == request.Id)
                .Select(article => new ArticleOutput
                {
                    Id = article.Id,
                    CreatedOnUtc = article.CreatedOnUtc,
                    Events = _dbContext
                        .ArticleEvents
                        .Where(articleEvent => articleEvent.ArticleId == article.Id)
                        .Select(articleEvent => new ArticleEventOutput
                        {
                            Id = articleEvent.Id,
                            EventType = articleEvent.EventType,
                            CreatedOnUtc = articleEvent.CreatedOnUtc
                        })
                        .ToList()
                })
                .FirstOrDefaultAsync(cancellationToken);


            var response = await _requestClient.GetResponse<ArticleResponse>(new
            {
                Id = articleResponse.Id
            });

            if (articleResponse is null)
            {
                return Result.Failure<ArticleOutput>(new Error(
                    "GetArticle.Null",
                    "The article with the specified ID was not found"));
            }

            return articleResponse;
        }
    }
}

public class ArticleOutput
{
    public Guid Id { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? PublishedOnUtc { get; set; }

    public List<ArticleEventOutput> Events { get; set; } = new();
}

public class ArticleEventOutput
{
    public Guid Id { get; set; }

    public DateTime CreatedOnUtc { get; set; }

    public ArticleEventType EventType { get; set; }
}

