using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newsletter.Reporting.Api.Features.Articles;

namespace Newsletter.Reporting.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArticleController : ControllerBase
    {
        private readonly ILogger<ArticleController> _logger;
        private readonly ISender _sender;
        public ArticleController(ILogger<ArticleController> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        [HttpGet]
        public async Task<ActionResult> Get(Guid id)
        {
            var query = new GetArticle.Query { Id = id };

            var result = await _sender.Send(query);

            if (result.IsFailure)
            {
                return NotFound(result.Error);
            }

            return Ok(result.Value);
        }
    }
}
