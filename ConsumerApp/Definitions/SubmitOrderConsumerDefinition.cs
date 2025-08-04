using ConsumerApp.Consumers;
using MassTransit;

namespace ConsumerApp.Definitions
{
    /// <summary>
    /// A class, derived from ConsumerDefinition<TConsumer> that configures settings and the consumer's receive endpoint
    /// </summary>
    public class SubmitOrderConsumerDefinition :
    ConsumerDefinition<SubmitOrderConsumer>
    {
        public SubmitOrderConsumerDefinition()
        {
            // override the default endpoint name, for whatever reason
            EndpointName = "ha-submit-order";

            // limit the number of messages consumed concurrently
            // this applies to the consumer only, not the endpoint
            ConcurrentMessageLimit = 4;
        }

        //protected override void ConfigureConsumer(IReceiveEndpointConfigurator endpointConfigurator,
        //    IConsumerConfigurator<DiscoveryPingConsumer> consumerConfigurator)
        //{
        //    endpointConfigurator.UseMessageRetry(r => r.Interval(5, 1000));
        //    endpointConfigurator.UseInMemoryOutbox();
        //}
    }
}
