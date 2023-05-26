using Taxbox.Domain.Entities.Common;
using MassTransit;
using System;
using System.Collections.Generic;

namespace Taxbox.Domain.Entities;

public class Article : Entity<ArticleId>
{
    public override ArticleId Id { get; set; } = NewId.NextGuid();
    public string Title { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IList<string>? Tags { get; set; }
}

public class Metadata
{
    // public string? Description { get; set; }
    // public string? Image { get; set; }
    // public string? Keywords { get; set; }
    // public string? Robots { get; set; }
    // public string? Title { get; set; }
    // public string? Url { get; set; }
    public string? Category { get; set; }
    public string? Language { get; set; }
    public int? Views { get; set; }
}