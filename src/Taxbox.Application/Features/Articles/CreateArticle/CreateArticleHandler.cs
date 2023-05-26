using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Result<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _esService;


    public CreateArticleHandler(IElasticSearchService<Article> esService)
    {
        _esService = esService;
    }

    public async Task<Result<GetArticleResponse>> Handle(CreateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var created = await _esService.AddOrUpdate(request.Adapt<Article>());
        return created.Adapt<GetArticleResponse>();
    }
}