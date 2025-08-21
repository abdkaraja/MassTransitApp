using MassTransit;

namespace Orchestrator.Api.Endpoints
{
    public interface IEndpointAddressProvider
    {
        Uri GetExecuteEndpoint<T, TArguments>()
        where T : class, IExecuteActivity<TArguments>
        where TArguments : class;
    }
}
