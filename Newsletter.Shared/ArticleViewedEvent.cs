namespace Newsletter.Shared
{
    public record ArticleViewedEvent
    {
        public Guid Id { get; set; }
        public DateTime ViewedOnUtc { get; set; }
    }
}
