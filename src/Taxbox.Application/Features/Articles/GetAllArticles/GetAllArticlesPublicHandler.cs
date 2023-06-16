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

public class
    GetAllArticlesPublicHandler : IRequestHandler<GetAllArticlesPublicRequest, PaginatedList<GetAllArticlesResponse>>
{
    private readonly IElasticSearchService<Article> _eSservice;

    public GetAllArticlesPublicHandler(IElasticSearchService<Article> eSservice)
    {
        _eSservice = eSservice;
    }

    public async Task<PaginatedList<GetAllArticlesResponse>> Handle(GetAllArticlesPublicRequest request,
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

        if (request.CurrentPage <= 0)
        {
            request.CurrentPage = 1;
        }

        if (request.PageSize <= 0)
        {
            request.PageSize = 10;
        }


        var fields = request.SourceFields?.Split(',').ToArray() ?? Array.Empty<string>();
        // Filter the fields to include only the properties that exist in YourModel
        var availableFields = typeof(Article).GetProperties().Select(p => p.Name);

        // Filter the requested fields to include only the available fields
        var validFields = fields.Intersect(availableFields, StringComparer.OrdinalIgnoreCase);

        var sort = new SortOptionsDescriptor<Article>().Field(article => article.UpdatedAt!,
            descriptor => descriptor.Order(SortOrder.Desc));
        var resp = await _eSservice.GetAllPaginated(qd, request.CurrentPage, request.PageSize, validFields.ToArray(),
            sort);

        var list = new List<GetAllArticlesResponse>();
        if (resp?.Hits != null)
        {
            foreach (var hit in resp.Hits)
            {
                if (hit.Source == null) continue;

                hit.Source.Id = Guid.Parse(hit.Id);

                list.Add(hit.Source.Adapt<GetAllArticlesResponse>());
            }

            return new PaginatedList<GetAllArticlesResponse>(list,
                (int)resp!.Total, request.CurrentPage, request.PageSize);
        }

        return new PaginatedList<GetAllArticlesResponse>();
    }

    private QueryDescriptor<Article> BuildQueryDescriptor(GetAllArticlesPublicRequest request)
    {
        var qd = new QueryDescriptor<Article>().Term(t => t.Field(f => f.IsPublic).Value(true));

        if (!string.IsNullOrEmpty(request.FreeTextSearch))
        {
            qd = qd.Bool(b =>
                b.Should(
                        m => m.Match(matchQueryDescriptor =>
                            matchQueryDescriptor.Field(f => f.Title).Query(request.FreeTextSearch)),
                        m => m.Match(matchQueryDescriptor =>
                            matchQueryDescriptor.Field(f => f.Content).Query(request.FreeTextSearch)))
                    .MinimumShouldMatch(1));
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            qd = qd.Match(m => m.Field(f => f.Title).Query(request.Title));
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            qd = qd.Match(m => m.Field(f => f.Content).Query(request.Content));
        }


        if (request.AuthorIds is { Count: > 0 })
        {
            var terms = new TermsQueryField(request.AuthorIds.Select(FieldValue.String).ToArray());
            qd = qd.Terms(t => t.Field(f => f.AuthorIds).Terms(terms));
        }

        if (request.Tags is { Count: > 0 })
        {
            var terms = new TermsQueryField(request.Tags.Select(FieldValue.String).ToArray());
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

        if (request.IsPublished != null)
        {
            qd = qd.Term(t => t.Field(f => f.IsPublished).Value((bool)request.IsPublished));
        }


        return qd;
    }
}