using MassTransit;
using Microsoft.AspNetCore.Mvc;
using RoutingSlipApi.Models.Events;

namespace RoutingSlipApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IBus _bus;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpPost]
        public async Task<ActionResult> Submit()
        {
            await _bus.Publish(new OrderSubmitted(
            OrderId: Guid.NewGuid(),
            CustomerEmail: "customer@example.com",
            Amount: 99.99m
            ));
            return Ok();
        }
    }
}

