using MassTransit;
using MassTransitApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnboardingController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<OnboardingController> _logger;
        public OnboardingController(ILogger<OnboardingController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }


        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe()
        {
            await _bus.Publish(new SubscribeToNewsletter("test@test.com"));
            return Ok("Subscribe successfully");
        }

    }
}
