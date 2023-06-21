using MassTransit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Domain.Entities;

public class Page : Entity<PageId>
{
    [Key]
    public override PageId Id { get; set; } = NewId.NextGuid();
    public string ParentName { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
    public string Label { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string? MetadataJson { get; set; } = "{ }";
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }

    [NotMapped]
    public Metadata? Metadata
    {
        get => JsonSerializer.Deserialize<Metadata>(MetadataJson ?? "{ }");
        set => MetadataJson = value is not null ? JsonSerializer.Serialize(value) : "{}";
    }
}