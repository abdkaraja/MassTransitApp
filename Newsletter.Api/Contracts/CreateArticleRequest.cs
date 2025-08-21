namespace Newsletter.Api.Contracts;

public class CreateArticleRequest
{
    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string Tags { get; set; } = string.Empty;
}