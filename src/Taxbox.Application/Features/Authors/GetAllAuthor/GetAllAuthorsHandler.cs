using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.GetAllAuthor;

public class GetAllAuthorsHandler : IRequestHandler<GetAllAuthorsRequest, PaginatedList<GetAuthorResponse>>
{
    private readonly IElasticSearchService<Author> _eSservice;
    private readonly IOptions<ElasticSearchConfiguration> _appSettings;

    public GetAllAuthorsHandler(IElasticSearchService<Author> eSservice,
        IOptions<ElasticSearchConfiguration> appSettings)
    {
        _eSservice = eSservice;
        _appSettings = appSettings;
    }

    public async Task<PaginatedList<GetAuthorResponse>> Handle(GetAllAuthorsRequest request,
        CancellationToken cancellationToken)
    {
        var qd = new QueryDescriptor<Author>();
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


        var resp = await _eSservice.Index(_appSettings.Value.AuthorsIndex)
            .GetAllPaginated(qd, request.CurrentPage, request.PageSize, fields);

        var list = new List<GetAuthorResponse>();
        if (resp?.Hits != null)
        {
            foreach (var hit in resp.Hits)
            {
                if (hit.Source == null) continue;

                hit.Source.Id = Guid.Parse(hit.Id);
                list.Add(hit.Source.Adapt<GetAuthorResponse>());
            }

            return new PaginatedList<GetAuthorResponse>(list,
                (int)resp!.Total, request.CurrentPage, request.PageSize);
        }

        return new PaginatedList<GetAuthorResponse>();
    }

    private QueryDescriptor<Author> BuildQueryDescriptor(GetAllAuthorsRequest request)
    {
        var qd = new QueryDescriptor<Author>();
        if (!string.IsNullOrEmpty(request.Name))
        {
            qd = qd.Match(m => m.Field(f => f.Name).Query(request.Name));
        }

        if (!string.IsNullOrEmpty(request.Bio))
        {
            qd = qd.Match(m => m.Field(f => f.Bio).Query(request.Bio));
        }

        if (!string.IsNullOrEmpty(request.Email))
        {
            qd = qd.Term(m => m.Field(f => f.Email).Value(request.Email));
        }

        if (request.JoinDate != null)
        {
            qd = qd.Range(
                q => q.DateRange(dateRangeQueryDescriptor => dateRangeQueryDescriptor.Field(f => f.JoinDate)
                    .Gte(request.JoinDate).Lte(request.JoinDate)));
        }

        if (request.BoundingBox != null)
        {
            qd = qd.GeoBoundingBox(g => g.Field(f => f.Location).BoundingBox(request.BoundingBox));
        }

        // if (request.SocialMedia != null)
        // {
        //     qd = qd.Nested(n => n.Path(p => p.SocialMedia).Query(q => q.Term(t => t.Field(f => f.SocialMedia)
        //         .Value(request.SocialMedia))));
        // }


        return qd;
    }
}