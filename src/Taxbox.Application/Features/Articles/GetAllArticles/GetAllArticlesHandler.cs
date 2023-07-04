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

public class GetAllArticlesHandler : IRequestHandler<GetAllArticlesRequest, PaginatedList<GetAllArticlesResponse>>
{
    private readonly IElasticSearchService<Article> _esService;

    public GetAllArticlesHandler(IElasticSearchService<Article> esService)
    {
        _esService = esService;
    }

    public async Task<PaginatedList<GetAllArticlesResponse>> Handle(GetAllArticlesRequest request,
        CancellationToken cancellationToken)
    {
        QueryContainer qd;
        if (string.IsNullOrEmpty(request.ToString()) || string.IsNullOrWhiteSpace(request.ToString()))
        {
            qd = new QueryContainerDescriptor<Article>().MatchAll();
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

        var availableFields = typeof(Article).GetProperties().Select(p => p.Name);
        IEnumerable<string> enumerable = availableFields as string[] ?? availableFields.ToArray();

        var fields = request.SourceFields?.Split(',').ToArray() ?? Array.Empty<string>();
        var validFields = fields.Intersect(enumerable, StringComparer.OrdinalIgnoreCase);

        var sd = new SearchDescriptor<Article>()
                .Index("articles")
                .Query(_ => qd)
                .From((request.CurrentPage - 1) * request.PageSize)
                .Size(request.PageSize)
                .Source(s => s.Includes(i => i.Fields(validFields.ToArray())))
            ;

        if (!string.IsNullOrEmpty(request.SortBy) && !string.IsNullOrWhiteSpace(request.SortBy))
        {
            if (!enumerable.Contains(request.SortBy, StringComparer.OrdinalIgnoreCase))
            {
                request.SortBy = GetAllArticlesRequestConstants.DefaultSortBy;
            }

            if (request.SortOrder != GetAllArticlesRequestConstants.Ascending &&
                request.SortOrder != GetAllArticlesRequestConstants.Descending)
            {
                request.SortOrder = GetAllArticlesRequestConstants.Descending;
            }

            var sort = new SortDescriptor<Article>().Field(request.SortBy,
                request.SortOrder == "asc" ? SortOrder.Ascending : SortOrder.Descending);

            sd.Sort(_ => sort);
        }

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

    private QueryContainer BuildQueryDescriptor(GetAllArticlesRequest request)
    {
        var should = new QueryContainer();
        var must = new QueryContainer();

        if (!string.IsNullOrEmpty(request.FreeTextSearch))
        {
            should = should || new MatchPhraseQuery()
            {
                Field = Infer.Field<Article>(f => f.Title), Query = request.FreeTextSearch, Boost = 3,
            };

            should = should || new MatchQuery
            {
                Field = Infer.Field<Article>(f => f.Title),
                Query = request.FreeTextSearch,
                Boost = 2,
                Fuzziness = Fuzziness.Auto,
                Operator = Operator.Or
            };

            should = should || new WildcardQuery
            {
                Field = Infer.Field<Article>(f => f.Title),
                Value = $"*{request.FreeTextSearch}*",
                Boost = 1,
                CaseInsensitive = true
            };

            should = should || new MatchPhraseQuery()
            {
                Field = Infer.Field<Article>(f => f.Content), Query = request.FreeTextSearch, Boost = 1,
            };

            // should = should || new WildcardQuery
            // {
            //     Field = Infer.Field<Article>(f => f.Content),
            //     Value = $"*{request.FreeTextSearch}*",
            //     Boost = 1,
            //     CaseInsensitive = true
            // };

            should = should || new MatchQuery
            {
                Field = Infer.Field<Article>(f => f.Content),
                Query = request.FreeTextSearch,
                Boost = 1,
                Fuzziness = Fuzziness.Auto,
                Operator = Operator.And
            };

            should = should && new BoolQuery { MinimumShouldMatch = 1 };
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

        if (request.IsPublic != null)
        {
            must = must && new TermQuery
            {
                Field = Infer.Field<Article>(f => f.IsPublic), Value = (bool)request.IsPublic
            };
        }

        if (request.Category != null)
        {
            must = must && new MatchQuery { Field = Infer.Field<Article>(f => f.Category), Query = request.Category };
        }

        if (request.Slug != null)
        {
            must = must && new TermQuery { Field = Infer.Field<Article>(f => f.Category), Value = request.Category };
        }


        return should && must;
    }
}