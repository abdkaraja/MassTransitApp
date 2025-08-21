namespace RoutingSlipApi.Models.Events
{
    public record EmailSent(Guid OrderId, string CustomerEmail);
}