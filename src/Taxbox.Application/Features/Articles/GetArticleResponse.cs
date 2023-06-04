using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles;

public record GetArticleResponse
{
    public ArticleId Id { get; init; }
    public string Title { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IList<string>? Tags { get; set; }
    public bool? IsPublic { get; set; } = false;
    public string? CoverImage { get; set; }
    public IList<ArticleAttachment>? Attachments { get; set; }
}