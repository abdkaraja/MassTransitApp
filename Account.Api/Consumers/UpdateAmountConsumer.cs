using Account.Api.Context;
using Account.Api.Models;
using MassTransit;
using SharedProj.Consumers;

namespace Account.Api.Consumers
{
    public class UpdateAmountConsumer : IConsumer<UpdateAmount>
    {
        private readonly AppDbContext _appDbContext;

        public UpdateAmountConsumer(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task Consume(ConsumeContext<UpdateAmount> context)
        {
            try
            {
                var source = _appDbContext.Set<User>().FirstOrDefault(x => x.Id == context.Message.From);
                if(source.Balance < context.Message.Amount)
                    await context.RespondAsync<UpdateAmountResponse>(new { Success = false, Message = "Insufficient balance" });
                else
                {
                    var distenation = _appDbContext.Set<User>().FirstOrDefault(x => x.Id == context.Message.To);
                    source.Balance -= context.Message.Amount + context.Message.Commission;
                    distenation.Balance += context.Message.Amount;
                    await _appDbContext.SaveChangesAsync();
                    await context.RespondAsync<UpdateAmountResponse>(new { Success = true });
                }
            }
            catch (Exception ex) 
            {
                await context.RespondAsync<UpdateAmountResponse>(new { Success = false, Message = ex.Message });
            }
        }
    }
}
