namespace SharedProj.Consumers
{
    public class TransferData
    {
        public Guid TransactionId { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Amount { get; set; }
        public bool IsTransactionDone { get; set; }
        public string Message { get; set; }
    }
}
