using Ardalis.Result;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Taxbox.Application.Features.Articles;
using Taxbox.Application.Features.Articles.CreateArticle;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Authors.UpdateAuthor;

public record UpdateAuthorRequest : IRequest<Result<GetAuthorResponse>>
{
    [JsonIgnore] public AuthorId Id { get; init; }

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public SocialMedia? SocialMedia { get; set; }
    public GeoLocation? Location { get; set; }
    public DateTime? JoinDate { get; set; }
}