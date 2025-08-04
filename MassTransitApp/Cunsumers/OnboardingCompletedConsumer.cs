using MassTransit;
using MassTransitApp.Models;

namespace MassTransitApp.Cunsumers
{
    public class OnboardingCompletedConsumer(ILogger<OnboardingCompletedConsumer> logger)
        : IConsumer<OnboardingCompleted>
    {
        public Task Consume(ConsumeContext<OnboardingCompleted> context)
        {
            logger.LogInformation("Onboarding Completed");
            return Task.CompletedTask;
        }
    }
}
