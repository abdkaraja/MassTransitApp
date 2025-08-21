namespace RoutingSlipApi.Models.Events
{
    public record PaymentProcessed(Guid OrderId, string TransactionId, decimal Amount);
}