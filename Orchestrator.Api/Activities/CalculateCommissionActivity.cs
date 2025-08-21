using MassTransit;
using SharedProj.Consumers;

namespace Orchestrator.Api.Activities
{
    public class CalculateCommissionActivity : IActivity<CalculateCommissionArguments, CalculateCommissionLog>
    {
        private readonly IRequestClient<CalculateCommissionInput> _requestClient;

        public CalculateCommissionActivity(IRequestClient<CalculateCommissionInput> requestClient)
        {
            _requestClient = requestClient;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<CalculateCommissionLog> context)
        {
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<CalculateCommissionArguments> context)
        {
            var calculateCommissionOutput = await _requestClient
                .GetResponse<CalculateCommissionOutput>(new CalculateCommissionInput { Amount = context.Arguments.Amount });

            var parameters = new Dictionary<string, object>();
            parameters.Add("Commission", calculateCommissionOutput.Message.Commission);
            
            return context.CompletedWithVariables(new
            {
                Amount = context.Arguments.Amount,
                Commission = calculateCommissionOutput.Message.Commission,
                Transactionid = context.Arguments.TransactionId
            }, parameters);
        }
    }

    public class CalculateCommissionArguments
    {
        public Guid TransactionId { get; set; }
        public int Amount { get; set; }
    }

    public interface CalculateCommissionLog
    {
        public Guid Transactionid { get; }
        public int Amount { get; }
        public int Commission { get; }
    }
}
