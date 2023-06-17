namespace Taxbox.Application.Features.Articles.BulkRemoveArticles;

public record BulkRemoveArticlesResponse
{
    public long? RemovedArticles { get; set; }
    public string Message { get; set; } = null!;
    public long? TotalArticles { get; set; }
}