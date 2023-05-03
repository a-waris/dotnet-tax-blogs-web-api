using Taxbox.Domain.Entities.Enums;
using MediatR;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;

namespace Taxbox.Application.Features.Heroes.GetAllHeroes;

public record GetAllHeroesRequest : PaginatedRequest, IRequest<PaginatedList<GetHeroResponse>>
{
    public string? Name { get; init; }
    public string? Nickname { get; init; }

    public int? Age { get; init; }

    public string? Individuality { get; init; }
    public HeroType? HeroType { get; init; }

    public string? Team { get; init; }
}