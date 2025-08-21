using Account.Api.Context;
using Account.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Account.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly AppDbContext _appDbContext;

        public AccountController(ILogger<AccountController> logger, AppDbContext appDbContext)
        {
            _logger = logger;
            _appDbContext = appDbContext;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<User> Get()
        {
            return _appDbContext.Set<User>().ToList();
        }

        [HttpPost(Name = "Post")]
        public IActionResult Post(User user)
        {
            user.Id = Guid.NewGuid();
            _appDbContext.Set<User>().Add(user);
            _appDbContext.SaveChanges();
            return Ok();
        }
    }
}
