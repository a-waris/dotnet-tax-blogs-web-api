using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Heroes;

public record GetHeroResponse
{
    public HeroId Id { get; init; }
    public string Name { get; init; } = null!;
    public string? Nickname { get; init; }

    public int? Age { get; init; }

    public string Individuality { get; init; } = null!;
    public HeroType? HeroType { get; init; }

    public string? Team { get; init; }
}