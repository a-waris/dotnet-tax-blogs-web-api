using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Mapster;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public class GetAllArticlesHandler : IRequestHandler<GetAllArticlesRequest, PaginatedList<GetArticleResponse>>
{
    private readonly IElasticSearchService<Article> _eSservice;

    public GetAllArticlesHandler(IElasticSearchService<Article> eSservice)
    {
        _eSservice = eSservice;
    }

    public async Task<PaginatedList<GetArticleResponse>> Handle(GetAllArticlesRequest request,
        CancellationToken cancellationToken)
    {
        var qd = new QueryDescriptor<Article>();
        if (string.IsNullOrEmpty(request.ToString()) || string.IsNullOrWhiteSpace(request.ToString()))
        {
            qd.MatchAll();
        }
        else
        {
            qd = BuildQueryDescriptor(request);
        }

        var resp = await _eSservice.GetAllPaginated(qd, request.CurrentPage, request.PageSize);

        var list = new List<GetArticleResponse>();
        if (resp?.Hits != null)
        {
            foreach (var hit in resp.Hits)
            {
                if (hit.Source == null) continue;

                hit.Source.Id = Guid.Parse(hit.Id);
                list.Add(hit.Source.Adapt<GetArticleResponse>());
            }

            return new PaginatedList<GetArticleResponse>(list,
                (int)resp!.Total, request.CurrentPage, request.PageSize);
        }

        return new PaginatedList<GetArticleResponse>();
    }

    private QueryDescriptor<Article> BuildQueryDescriptor(GetAllArticlesRequest request)
    {
        var qd = new QueryDescriptor<Article>();
        if (!string.IsNullOrEmpty(request.Title))
        {
            qd = qd.Match(m => m.Field(f => f.Title).Query(request.Title));
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            qd = qd.Match(m => m.Field(f => f.Content).Query(request.Content));
        }

        if (!string.IsNullOrEmpty(request.Author))
        {
            qd = qd.Match(m => m.Field(f => f.Author).Query(request.Author));
        }

        if (request.Tags is { Count: > 0 })
        {
            var terms = new TermsQueryField(request.Tags.Select(id => FieldValue.String(id)).ToArray());
            qd = qd.Terms(t => t.Field(f => f.Tags).Terms(terms));
        }

        if (request.CreatedAt != null)
        {
            qd = qd.Range(
                q => q.DateRange(dateRangeQueryDescriptor => dateRangeQueryDescriptor.Field(f => f.CreatedAt)
                    .Gte(request.CreatedAt).Lte(request.CreatedAt)));
        }

        if (request.UpdatedAt != null)
        {
            qd = qd.Range(
                q => q.DateRange(dateRangeQueryDescriptor => dateRangeQueryDescriptor.Field(f => f.UpdatedAt)
                    .Gte(request.UpdatedAt).Lte(request.UpdatedAt)));
        }

        return qd;
    }
}