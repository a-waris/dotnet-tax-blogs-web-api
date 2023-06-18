using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.BulkAddOrUpdateArticles;

public class BulkAddOrUpdateArticlesHandler : IRequestHandler<BulkAddOrUpdateArticlesRequest, Result<BulkAddOrUpdateArticlesResponse>>
{
    private readonly IElasticSearchService<Article> _esService;

    public BulkAddOrUpdateArticlesHandler(IElasticSearchService<Article> esService)
    {
        _esService = esService;
    }


    public async Task<Result<BulkAddOrUpdateArticlesResponse>> Handle(BulkAddOrUpdateArticlesRequest request,
        CancellationToken cancellationToken)
    {
        var resp = new BulkAddOrUpdateArticlesResponse();
        var bulkResponse = await _esService.AddOrUpdateBulk(request.Articles);
        resp.UpsertedArticles = bulkResponse.Items.Count;
        resp.Message = "Articles updated successfully";

        return resp;
    }
}