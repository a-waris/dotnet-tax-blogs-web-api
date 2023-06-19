using Ardalis.Result;
using Nest;
using System;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Authors.UpdateAuthor;

public record UpdateAuthorRequest : MediatR.IRequest<Result<GetAuthorResponse>>
{
    [JsonIgnore] public string Id { get; init; } = null!;

    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public SocialMedia? SocialMedia { get; set; }
    public GeoLocation? Location { get; set; }
    public DateTime? JoinDate { get; set; }
}