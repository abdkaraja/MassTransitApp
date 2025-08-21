using Contract.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Contract.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommissionController : ControllerBase
    {
        private readonly ContractDbContext _contractContext;

        public CommissionController(ContractDbContext contractContext)
        {
            _contractContext = contractContext;
        }

        [HttpPost]
        public async Task<IActionResult> Transfer(Commission commission)
        {
            try
            {
                _contractContext.Set<Commission>().Add(commission);
                await _contractContext.SaveChangesAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
