using Elastic.Clients.Elasticsearch;

namespace Taxbox.Application.Features.Articles.BulkAddArticles;

public record BulkAddArticlesResponse
{
    public int InvalidRows { get; set; }
    public int SuccessfulRows { get; set; }
    public int TotalRows { get; set; }
}