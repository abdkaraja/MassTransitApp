namespace RoutingSlipApi.Models.Arguments
{
    public record SendConfirmationEmailArguments
    {
        public Guid OrderId { get; init; }
        public string CustomerEmail { get; init; }
        public string TransactionId { get; init; }
    }
}
