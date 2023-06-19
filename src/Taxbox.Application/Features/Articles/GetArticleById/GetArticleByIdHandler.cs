using Ardalis.Result;
using Mapster;
using MediatR;
using Nest;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;
using Result = Ardalis.Result.Result;

namespace Taxbox.Application.Features.Articles.GetArticleById;

public class GetArticleByIdHandler : IRequestHandler<GetArticleByIdRequest, Result<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _esService;
    private readonly IElasticSearchService<Author> _esServiceAuthor;

    public GetArticleByIdHandler(IElasticSearchService<Article> esService,
        IElasticSearchService<Author> esServiceAuthor)
    {
        _esService = esService;
        _esServiceAuthor = esServiceAuthor;
    }


    public async Task<Result<GetArticleResponse>> Handle(GetArticleByIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _esService.Get(request.Id.ToString()!);
        if (result.Source == null)
        {
            return Result.NotFound();
        }

        result.Source.Id = result.Id;
        //get all authors for this article
        if (result.Source.AuthorIds == null)
        {
            return result.Source.Adapt<GetArticleResponse>();
        }

        var sd = new SearchDescriptor<Author>()
            .Query(q => q.Term(t => t.Field(f => f.Id.Suffix("keyword")).Value("taxbox")));
        var taxboxAuthorResp = await _esServiceAuthor.Index("authors").Query(sd);

        var authorsResp = await _esServiceAuthor.Index("authors").Query(new SearchDescriptor<Author>()
            .Query(q => q.Terms(t => t.Field(f => f.Id).Terms(result.Source.AuthorIds))));
        if (authorsResp is { IsValid: true } && authorsResp.Documents.Any())
        {
            result.Source.Authors = authorsResp.Documents.ToList();
        }

        return result.Source.Adapt<GetArticleResponse>();
    }
}