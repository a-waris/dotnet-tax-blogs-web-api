using Mapster;
using MediatR;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public class GetAllArticlesHandler : IRequestHandler<GetAllArticlesRequest, IList<GetArticleResponse>>
{
    // private readonly IElasticSearchService<Article> _client;
    //
    // public GetAllArticlesHandler(IElasticSearchService<Article> client)
    // {
    //     _client = client;
    // }

    public async Task<IList<GetArticleResponse>> Handle(GetAllArticlesRequest request,
        CancellationToken cancellationToken)
    {
        // var result = await _client.GetAll<Article>();
        // return result != null ? result.Adapt<IList<GetArticleResponse>>() : new List<GetArticleResponse>();
        return new List<GetArticleResponse>();
    }
}