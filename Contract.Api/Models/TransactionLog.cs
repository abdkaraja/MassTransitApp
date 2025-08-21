using SharedProj;

namespace Contract.Api.Models
{
    public class TransactionLog : EntityBase
    {
        public Guid TransactionId { get; set; }
        public int Commission { get; set; }
        public int Amount { get; set; }
        public Guid From { get; set; }
        public Guid To { get; set; }
    }
}
