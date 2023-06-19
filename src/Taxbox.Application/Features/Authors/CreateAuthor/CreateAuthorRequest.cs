using Ardalis.Result;
using Nest;
using System;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.CreateAuthor;

public record CreateAuthorRequest : MediatR.IRequest<Result<GetAuthorResponse>>
{
    
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Bio { get; set; } = null!;
    public SocialMedia? SocialMedia { get; set; }
    public GeoLocation? Location { get; set; }
    public DateTime? JoinDate { get; set; }
}