using Elastic.Clients.Elasticsearch.Mapping;
using MassTransit;
using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Authors;

public record GetAuthorResponse
{
    public AuthorId Id { get; init; }
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public SocialMedia? SocialMedia { get; set; }
    public GeoPointProperty? Location { get; set; }
    public DateTime? JoinDate { get; set; }
}