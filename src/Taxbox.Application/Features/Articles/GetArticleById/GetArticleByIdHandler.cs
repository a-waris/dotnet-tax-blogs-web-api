using Ardalis.Result;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Mapster;
using MediatR;
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
    private readonly IElasticSearchService<Article> _eSservice;
    private readonly IElasticSearchService<Author> _eSserviceAuthor;

    public GetArticleByIdHandler(IElasticSearchService<Article> eSservice,
        IElasticSearchService<Author> eSserviceAuthor)
    {
        _eSservice = eSservice;
        _eSserviceAuthor = eSserviceAuthor;
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
        //get all authors for this article
        if (result.Source.AuthorIds == null)
        {
            return result.Source.Adapt<GetArticleResponse>();
        }

        var terms = new TermsQueryField(result.Source.AuthorIds.Select(FieldValue.String).ToArray());
        var qd = new QueryDescriptor<Author>().Terms(t => t.Field(f => f.AuthorId).Terms(terms));
        var authorsResp = await _eSserviceAuthor.Index("authors").Query(qd);
        if (authorsResp.IsValidResponse && authorsResp.Documents.Any())
        {
            result.Source.Authors = authorsResp.Documents.ToList();
        }

        return result.Source.Adapt<GetArticleResponse>();
    }
}