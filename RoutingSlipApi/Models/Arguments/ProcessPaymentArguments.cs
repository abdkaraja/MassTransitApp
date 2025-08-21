namespace RoutingSlipApi.Models.Arguments
{
    // ===== ACTIVITY ARGUMENTS =====
    public record ProcessPaymentArguments
    {
        public Guid OrderId { get; init; }
        public decimal Amount { get; init; }
        public string CustomerEmail { get; init; }
    }
}
