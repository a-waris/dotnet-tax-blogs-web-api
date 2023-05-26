using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public class GetAllArticlesHandler : IRequestHandler<GetAllArticlesRequest, IList<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _eSservice;

    public GetAllArticlesHandler(IElasticSearchService<Article> eSservice)
    {
        _eSservice = eSservice;
    }

    public async Task<IList<GetArticleResponse>> Handle(GetAllArticlesRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _eSservice.GetAll();
        return result != null ? result.Adapt<IList<GetArticleResponse>>() : new List<GetArticleResponse>();
    }
}