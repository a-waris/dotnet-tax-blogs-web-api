namespace Taxbox.Application.Features.Articles.BulkAddOrUpdateArticles;

public record BulkAddOrUpdateArticlesResponse
{
    public string Message { get; set; } = null!;
    public long UpsertedArticles { get; set; }
}