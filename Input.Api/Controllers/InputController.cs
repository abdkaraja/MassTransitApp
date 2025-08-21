using Input.Api.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using SharedProj.Consumers;
using System.Threading.Tasks;

namespace Input.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InputController : ControllerBase
    {
        private readonly ILogger<InputController> _logger;
        private readonly IRequestClient<TransferData> _requestClient;

        public InputController(ILogger<InputController> logger, IRequestClient<TransferData> requestClient)
        {
            _logger = logger;
            _requestClient = requestClient;
        }

        [HttpPost("Transfer")]
        public async Task<IActionResult> Transfer(TransferInput data)
        {
            try
            {
                var transferData = new TransferData
                {
                    Amount = data.Amount,
                    From = data.From,
                    To = data.To,
                    TransactionId = Guid.NewGuid()
                };
                var response = await _requestClient.GetResponse<TransferData>(transferData, new System.Threading.CancellationToken(), RequestTimeout.After(1, 1, 1, 1));
                return Ok(response.Message.IsTransactionDone);
            }
            catch (Exception ex) 
            {
                return NotFound(ex.Message);
            }
        }
    }
}
