namespace ConsumerApp.Models
{
    public record SubmitOrder
    {
        public Guid OrderId { get; set; }
        public string Sku { get; init; }
        public int Quantity { get; init; }
    }
}
