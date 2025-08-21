using Automatonymous;
using MassTransit;
using SharedProj.Consumers;

namespace Orchestrator.Api.Activities
{
    public class AddTransactionLogActivity : IActivity<AddTransactionLogArguments, AddTransactionLog>
    {
        private IRequestClient<AddTransactionLogInput> _requestClient;

        public AddTransactionLogActivity(IRequestClient<AddTransactionLogInput> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<AddTransactionLog> context)
        {
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<AddTransactionLogArguments> context)
        {
            var commission = context.GetVariable<int>("Commission");
            var response = await _requestClient.GetResponse<AddTransactionLogOutput>(new AddTransactionLogInput
            {
                Amount = context.Arguments.Amount,
                Commission = commission == null ? 0 : commission.Value,
                From = context.Arguments.From,
                To = context.Arguments.To,
                TransactionId = context.Arguments.TransactionId
            });

            if(!response.Message.Success)
                throw new InvalidDataException("Error Error");

            return context.Completed<AddTransactionLog>(new
            {
                TransactionId = context.Arguments.TransactionId
            });
        }
    }

    public class AddTransactionLogArguments
    {
        public Guid TransactionId { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Commission { get; set; }
        public int Amount { get; set; }
    }

    public interface AddTransactionLog
    {
        public Guid TransactionId { get; set; }
    }
}
