namespace SharedProj.Consumers
{
    public class UpdateAmountCompensate
    {
        public Guid TransactionId { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Amount { get; set; }
        public int Commission { get; set; }
    }
}
