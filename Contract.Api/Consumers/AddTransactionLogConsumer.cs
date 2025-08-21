using Contract.Api.Models;
using MassTransit;
using SharedProj.Consumers;

namespace Contract.Api.Consumers
{
    public class AddTransactionLogConsumer : IConsumer<AddTransactionLogInput>
    {
        private ContractDbContext _contractDbContext;

        public AddTransactionLogConsumer(ContractDbContext context)
        {
            _contractDbContext = context;
        }

        public async Task Consume(ConsumeContext<AddTransactionLogInput> context)
        {
            try
            {
                //await context.RespondAsync<AddTransactionLogOutput>(new
                //{
                //    Success = false,
                //    Message = "Error"
                //});

                _contractDbContext.Set<TransactionLog>().Add(new TransactionLog
                {
                    Amount = context.Message.Amount,
                    Commission = context.Message.Commission,
                    From = context.Message.From,
                    To = context.Message.To,
                    TransactionId = context.Message.TransactionId
                });

                await _contractDbContext.SaveChangesAsync();
            }
            catch (Exception ex) 
            {
                throw new Exception("An error occurred while adding a new transaction log");
            }
        }
    }
}
