using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;
using MassTransit;

namespace Taxbox.Domain.Entities;

public class Hero : Entity<HeroId>
{
    public override HeroId Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; } = null!;

    public string? Nickname { get; set; }
    public string? Individuality { get; set; } = null!;
    public int? Age { get; set; }

    public HeroType HeroType { get; set; }

    public string? Team { get; set; }
}