namespace Taxbox.Application.Features.Articles.BulkRemoveArticles;

public record BulkRemoveArticlesResponse
{
    public long? RemovedArticles { get; set; }
    public string Message { get; set; } = null!;
    public long? TotalArticlesFound { get; set; }
    public long? ArticlesNotFound { get; set; }
}