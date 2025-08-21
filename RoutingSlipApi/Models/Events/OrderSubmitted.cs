namespace RoutingSlipApi.Models.Events
{
    public record OrderSubmitted(Guid OrderId, string CustomerEmail, decimal Amount);
}