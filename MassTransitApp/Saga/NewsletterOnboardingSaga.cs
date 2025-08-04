using MassTransit;
using MassTransitApp.Models;

namespace MassTransitApp.Saga
{
    public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
    {
        public State Welcomeing { get; set; }
        public State FollowingUp { get; set; }
        public State Onboarding { get; set; }

        public Event<SubscriberCreated> SubscriberCreated { get;set; }
        public Event<WelcomeEmailSent> WelcomeEmailSent { get;set; }
        public Event<FollowUpEmailSent> FollowUpEmailSent { get;set; }
        //public Event<OnboardingCompleted> OnboardingCompleted { get;set; }

        public NewsletterOnboardingSaga()
        {
            InstanceState(x => x.CurrentState);

            Event(() => SubscriberCreated, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => WelcomeEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
            Event(() => FollowUpEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));


        }
    }
}
