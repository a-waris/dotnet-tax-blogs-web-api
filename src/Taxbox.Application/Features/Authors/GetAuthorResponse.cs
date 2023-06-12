using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using System;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Authors;

public record GetAuthorResponse
{
    public AuthorId AuthorId { get; init; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public SocialMedia? SocialMedia { get; set; }
    public GeoLocation? Location { get; set; }
    public DateTime? JoinDate { get; set; }
}