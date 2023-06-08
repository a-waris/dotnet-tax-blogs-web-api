using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;
using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxbox.Domain.Entities;

[Table("Resources")]
public class Resource : Entity<ResourceId>
{
    public override ResourceId Id { get; set; } = NewId.NextGuid();
    public string DisplayName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public ResourceType ResourceType { get; set; }
    public CategoryId CategoryId { get; set; }
    [ForeignKey(nameof(CategoryId))] public Category? Category { get; init; }
}