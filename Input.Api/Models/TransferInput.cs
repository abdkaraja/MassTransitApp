namespace Input.Api.Models
{
    public class TransferInput
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Amount { get; set; }
    }
}
