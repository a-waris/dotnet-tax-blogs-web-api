using Mapster;
using MediatR;
using Nest;
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
    private readonly IElasticSearchService<Article> _esService;

    public GetAllArticlesPublicHandler(IElasticSearchService<Article> esService)
    {
        _esService = esService;
    }

    public async Task<PaginatedList<GetAllArticlesResponse>> Handle(GetAllArticlesPublicRequest request,
        CancellationToken cancellationToken)
    {
        var qd = BuildQueryDescriptor(request);

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

        var sd = new SearchDescriptor<Article>()
                .Index("articles")
                .Query(_ => qd)
                .Sort(s => s.Descending(a => a.UpdatedAt))
                .From((request.CurrentPage - 1) * request.PageSize)
                .Size(request.PageSize)
                .Source(s => s.Includes(i => i.Fields(validFields.ToArray())))
            ;
        var resp = await _esService.Query(sd);

        var list = new List<GetAllArticlesResponse>();
        if (resp?.Hits != null)
        {
            foreach (var hit in resp.Hits)
            {
                if (hit.Source == null) continue;
                hit.Source.Id = hit.Id;
                list.Add(hit.Source.Adapt<GetAllArticlesResponse>());
            }

            return new PaginatedList<GetAllArticlesResponse>(list,
                (int)resp.Total, request.CurrentPage, request.PageSize);
        }

        return new PaginatedList<GetAllArticlesResponse>();
    }

    public static QueryContainer BuildQueryDescriptor(GetAllArticlesPublicRequest request)
    {
        var should = new QueryContainer();
        var must = new QueryContainer();

        if (!string.IsNullOrEmpty(request.FreeTextSearch))
        {
            should = should || new MatchQuery
            {
                Field = Infer.Field<Article>(f => f.Title), Query = request.FreeTextSearch, Boost = 2
            };
            should = should || new MatchQuery
            {
                Field = Infer.Field<Article>(f => f.Content), Query = request.FreeTextSearch, Boost = 1
            };
            should = should || new BoolQuery { MinimumShouldMatch = 1 };
        }

        if (!string.IsNullOrEmpty(request.Title))
        {
            must = must && new MatchQuery
            {
                Field = Infer.Field<Article>(f => f.Title), Query = request.Title, Boost = 2
            };
        }

        if (!string.IsNullOrEmpty(request.Content))
        {
            must = must && new MatchQuery
            {
                Field = Infer.Field<Article>(f => f.Content), Query = request.Content, Boost = 1
            };
        }


        if (request.AuthorIds is { Count: > 0 })
        {
            must = must && new TermsQuery { Field = Infer.Field<Article>(f => f.AuthorIds), Terms = request.AuthorIds };
        }

        if (request.Tags is { Count: > 0 })
        {
            must = must && new TermsQuery { Field = Infer.Field<Article>(f => f.Tags), Terms = request.Tags };
        }

        if (request.CreatedAt != null)
        {
            must = must && new DateRangeQuery
            {
                Field = Infer.Field<Article>(f => f.CreatedAt),
                GreaterThanOrEqualTo = request.CreatedAt,
                LessThanOrEqualTo = request.CreatedAt
            };
        }

        if (request.UpdatedAt != null)
        {
            must = must && new DateRangeQuery
            {
                Field = Infer.Field<Article>(f => f.UpdatedAt),
                GreaterThanOrEqualTo = request.UpdatedAt,
                LessThanOrEqualTo = request.UpdatedAt
            };
        }

        if (request.IsPublished != null)
        {
            must = must && new TermQuery
            {
                Field = Infer.Field<Article>(f => f.IsPublished), Value = (bool)request.IsPublished
            };
        }
        
        var termIsPublic = new TermQuery
        {
            Field = Infer.Field<Article>(f => f.IsPublic), Value = true
        };

        return should && must && termIsPublic;
    }
}