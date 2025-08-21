using Contract.Api.Models;
using MassTransit;
using SharedProj.Consumers;

namespace Contract.Api.Consumers
{
    public class CalculateCommissionConsumer : IConsumer<CalculateCommissionInput>
    {
        private ContractDbContext _context;

        public CalculateCommissionConsumer(ContractDbContext context)
        {
            _context = context;
        }

        public async Task Consume(ConsumeContext<CalculateCommissionInput> context)
        {
            var amount = context.Message.Amount;
            var commission = _context.Set<Commission>()
                .FirstOrDefault(x => x.FromAmount < amount && x.ToAmount >= amount);

            if (commission is null)
                throw new InvalidDataException("Commission not found");

            await context.RespondAsync<CalculateCommissionOutput>(new
            {
                Commission = (amount * commission.Percentage) / 100
            });
        }
    }
}
