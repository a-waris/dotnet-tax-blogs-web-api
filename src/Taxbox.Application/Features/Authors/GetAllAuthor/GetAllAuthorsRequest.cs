using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.GetAllAuthor;

public record GetAllAuthorsRequest : PaginatedRequest, IRequest<PaginatedList<GetAuthorResponse>>
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Bio { get; set; }
    public SocialMedia? SocialMedia { get; set; }
    public GeoBounds? BoundingBox { get; set; }
    public DateTime? JoinDate { get; set; }
    public string? SourceFields { get; set; }

    public override string ToString()
    {
        return
            $"{Name} {Email} {Bio} {SocialMedia} {BoundingBox} {JoinDate} {SourceFields}";
    }
}