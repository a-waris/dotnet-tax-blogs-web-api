using Ardalis.Result;
using MediatR;
using Nest;
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
        
        var deleteQuery = new DeleteByQueryRequest<Article>("articles") { Query = new TermsQuery
            {
                Field = "_id",
                Terms = request.ArticleIds
            }
        };
        var bulkResponse = await _esService.BulkRemove(deleteQuery);
        resp.RemovedArticles = bulkResponse.Deleted;
        resp.TotalArticlesFound = bulkResponse.Total;
        resp.ArticlesNotFound = bulkResponse.Noops;
        resp.Message = "Articles removed successfully";

        return resp;
    }
}