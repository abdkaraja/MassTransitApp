using MassTransit;
using Orchestrator.Api.Saga;
using SharedProj.Consumers;

namespace Orchestrator.Api.Activities
{
    public class CalculateCommissionActivity : IActivity<CalculateCommissionArguments, CalculateCommissionLog>
    {
        private readonly IRequestClient<CalculateCommissionInput> _requestClient;
        private readonly IPublishEndpoint _publishEndpoint;

        public CalculateCommissionActivity(IRequestClient<CalculateCommissionInput> requestClient, IPublishEndpoint publishEndpoint)
        {
            _requestClient = requestClient;
            _publishEndpoint = publishEndpoint;
        }

        public async Task<CompensationResult> Compensate(CompensateContext<CalculateCommissionLog> context)
        {
            return context.Compensated();
        }

        public async Task<ExecutionResult> Execute(ExecuteContext<CalculateCommissionArguments> context)
        {
            await _publishEndpoint.Publish(new StartTransactionEvent()
            {
                TransactionId = context.Arguments.TransactionId
            });

            var calculateCommissionOutput = await _requestClient
                .GetResponse<CalculateCommissionOutput>(new CalculateCommissionInput { Amount = context.Arguments.Amount });

            var parameters = new Dictionary<string, object>();
            parameters.Add("Commission", calculateCommissionOutput.Message.Commission);

            await _publishEndpoint.Publish(new OnCommissionCalculateEvent()
            {
                TransactionId = context.Arguments.TransactionId
            });

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
