using Ardalis.Result;
using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Features.Heroes;
using Taxbox.Application.Features.Heroes.GetHeroById;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetArticleById;

public class GetArticleByIdHandler : IRequestHandler<GetArticleByIdRequest, Result<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _eSservice;

    public GetArticleByIdHandler(IElasticSearchService<Article> eSservice)
    {
        _eSservice = eSservice;
    }


    public async Task<Result<GetArticleResponse>> Handle(GetArticleByIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _eSservice.Get(request.Id.ToString()!);
        if (result.Source == null)
        {
            return Result.NotFound();
        }

        result.Source.Id = Guid.Parse(result.Id);
        return result.Source.Adapt<GetArticleResponse>();

    }
}