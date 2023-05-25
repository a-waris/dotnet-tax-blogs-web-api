using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public class CreateArticleHandler : IRequestHandler<CreateArticleRequest, Result<GetArticleResponse>>
{
    // private readonly IElasticSearchService<Article> _client;


    // public CreateArticleHandler(IElasticSearchService<Article> client)
    // {
    //     _client = client;
    // }

    public async Task<Result<GetArticleResponse>> Handle(CreateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Article>();
        // await _client.AddOrUpdate(created);
        return created.Adapt<GetArticleResponse>();
    }
}