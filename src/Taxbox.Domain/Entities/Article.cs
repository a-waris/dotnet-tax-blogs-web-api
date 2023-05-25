using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;
using MassTransit;
using System.Collections.Generic;

namespace Taxbox.Domain.Entities;

public class Article : Entity<ArticleId>
{
    public override ArticleId Id { get; set; } = NewId.NextGuid();
    public string Title { get; set; } = null!;
    public string? Metadata { get; set; }
    public string? Content { get; set; } = null!;
    public string? Author { get; set; } = null!;
    public string? Date { get; set; } = null!;
    public IList<string>? Tags { get; set; } = null!;
}