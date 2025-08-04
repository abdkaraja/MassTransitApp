using MassTransit;

namespace MassTransitApp.Saga
{
    public class NewsletterOnboardingSagaData : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string CurrentState { get; set; }

        public string Email { get; set; }
        public Guid SubscriberId { get; set; }
        public bool WelcomeEmailSent { get; set; }
        public bool FollowUpEmailSent { get; set; }
        public bool OnboardingCompleted { get; set; }

    }
}
