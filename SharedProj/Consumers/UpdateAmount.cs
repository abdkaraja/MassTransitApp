namespace SharedProj.Consumers
{
    public class UpdateAmount
    {
        public Guid From { get; set; }
        public Guid To { get; set; }
        public int Amount { get; set; }
        public int Commission { get; set; }
    }
}
