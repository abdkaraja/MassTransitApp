using Automatonymous;
using MassTransit;
using MassTransit.Transports;
using SharedProj.Consumers;

namespace Orchestrator.Api.Activities
{
    public class UpdateAmountActivity : IActivity<UpdateAmountArguments, UpdateAmountLog>
    {
        private IRequestClient<UpdateAmount> _requestClient;
        private IRequestClient<UpdateAmountCompensate> _requestClientCompensate;

        public UpdateAmountActivity(IRequestClient<UpdateAmount> requestClient, IRequestClient<UpdateAmountCompensate> requestClientCompensate)
        {
            _requestClient = requestClient;
            _requestClientCompensate = requestClientCompensate;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<UpdateAmountLog> context)
        {
            var log = context.Log;
            var response = await _requestClientCompensate.GetResponse<UpdateAmountResponse>(new UpdateAmountCompensate()
            {
                Amount = context.Log.Amount,
                From = context.Log.From,
                To = context.Log.To,
                Commission = context.Log.Commission,
                TransactionId = context.Log.TransactionId,
            });
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<UpdateAmountArguments> context)
        {
            var commission = context.GetVariable<int>("Commission");

            var response = await _requestClient.GetResponse<UpdateAmountResponse>(new UpdateAmount()
            {
                Amount = context.Arguments.Amount,
                From = context.Arguments.From,
                To = context.Arguments.To,
                Commission = commission == null ? 0 : commission.Value,
            });

            if (!response.Message.Success)
            {
                throw new InvalidDataException("Insufficient balance");
            }
            return context.Completed<UpdateAmountLog>(new
            {
                TransactionId = context.Arguments.TransactionId,
                Amount = context.Arguments.Amount,
                From = context.Arguments.From,
                To = context.Arguments.To,
                Commission = commission == null ? 0 : commission.Value,
            });
        }
    }

    public class UpdateAmountArguments
    {
        public Guid TransactionId { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Amount { get; set; }
        
    }
    public interface UpdateAmountLog
    {
        public Guid TransactionId { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Amount { get; set; }
        public int Commission { get; set; }
    }
}
