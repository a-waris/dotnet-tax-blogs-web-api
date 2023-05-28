using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.UpdateArticle;

public class UpdateArticleHandler : IRequestHandler<UpdateArticleRequest, Result<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _eSservice;

    public UpdateArticleHandler(IElasticSearchService<Article> eSservice)
    {
        _eSservice = eSservice;
    }


    public async Task<Result<GetArticleResponse>> Handle(UpdateArticleRequest request,
        CancellationToken cancellationToken)
    {
        var existingArticle = await _eSservice.Get(request.Id.Adapt<string>());
        if (existingArticle.Source == null)
        {
            return Result.NotFound();
        }
        var result = await _eSservice.AddOrUpdate(request.Adapt(existingArticle.Source));
        return result.Adapt<GetArticleResponse>();
    }
}