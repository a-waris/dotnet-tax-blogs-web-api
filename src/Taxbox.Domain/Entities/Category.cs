using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;
using MassTransit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Taxbox.Domain.Entities;

[Table("Categories")]
public class Category : Entity<CategoryId>
{
    public override CategoryId Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Status { get; set; } = true;
}