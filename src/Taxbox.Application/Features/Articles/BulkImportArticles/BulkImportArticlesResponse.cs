namespace Taxbox.Application.Features.Articles.BulkImportArticles;

public record BulkImportArticlesResponse
{
    public int InvalidRows { get; set; }
    public int SuccessfulRows { get; set; }
    public int TotalRows { get; set; }
}