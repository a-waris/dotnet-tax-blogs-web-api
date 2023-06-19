using System;
using System.Collections.Generic;

namespace Taxbox.Domain.Entities;

public class Article
{
    public string Id { get; set; } = null!;
    public string Title { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string? HtmlContent { get; set; }
    public string? Content { get; set; }
    public IList<string>? AuthorIds { get; set; } = new List<string>();
    public IList<Author>? Authors { get; set; } = new List<Author>();
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public IList<string>? Tags { get; set; }
    public bool IsPublic { get; set; }
    public bool IsPublished { get; set; }
    public bool IsDraft { get; set; }
    public string? CoverImage { get; set; }
    public string? ThumbnailImage { get; set; }
    public IList<ArticleAttachment>? Attachments { get; set; }
}

public class ArticleAttachment
{
    public string File { get; set; } = null!;
    public string Type { get; set; } = null!;
}

public class Metadata
{
    public string? Category { get; set; }
    public string? Language { get; set; }
    public int? Views { get; set; }

    public override string ToString()
    {
        return $"{Category} {Language} {Views}";
    }
}