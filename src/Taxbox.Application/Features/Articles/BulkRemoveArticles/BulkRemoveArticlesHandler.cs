using Ardalis.Result;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Mapster;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.BulkRemoveArticles;

public class BulkRemoveArticlesHandler : IRequestHandler<BulkRemoveArticlesRequest, Result<BulkRemoveArticlesResponse>>
{
    private readonly IElasticSearchService<Article> _esService;

    public BulkRemoveArticlesHandler(IElasticSearchService<Article> esService)
    {
        _esService = esService;
    }


    public async Task<Result<BulkRemoveArticlesResponse>> Handle(BulkRemoveArticlesRequest request,
        CancellationToken cancellationToken)
    {
        var resp = new BulkRemoveArticlesResponse();
        var qd = new QueryDescriptor<Article>();
        var terms = new TermsQueryField(request.ArticleIds.Select(FieldValue.String).ToArray());
        qd.Terms(article => article.Field("id").Terms(terms));
        var bulkResponse = await _esService.BulkRemove(qd);
        resp.RemovedArticles = bulkResponse.Deleted;
        resp.TotalArticles = bulkResponse.Total;
        resp.Message = "Articles removed successfully";

        return resp;
    }
}