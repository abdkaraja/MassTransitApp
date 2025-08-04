using MassTransit;
using MassTransitApp.Models;
using MassTransitApp.Models.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MassTransitApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OnboardingController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<OnboardingController> _logger;
        private readonly IRequestClient<SubmitOrder> _client;
        public OnboardingController(ILogger<OnboardingController> logger, IRequestClient<SubmitOrder> client, IBus bus)
        {
            _logger = logger;
            _client = client;
            _bus = bus;
        }


        //[HttpPost("submit")]
        //public async Task<IActionResult> SubmitOrder()
        //{
        //    var order = new SubmitOrder(Guid.NewGuid(), "Laptop", 2);
        //    await _bus.Publish(order);
        //    return Ok("Order submitted");
        //}


       
    }
}
