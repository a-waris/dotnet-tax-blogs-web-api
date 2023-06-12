using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles;

public record GetArticleResponse
{
    public ArticleId ArticleId { get; init; }

    public string Title { get; set; } = null!;

    public Metadata? Metadata { get; set; }

    public string Content { get; set; } = null!;

    public IList<string>? AuthorIds { get; set; } = new List<string>();
    public IList<Author>? Authors { get; set; } = new List<Author>();

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public IList<string>? Tags { get; set; }

    public bool IsPublic { get; set; } = false;

    public bool IsPublished { get; set; } = false;

    public string? CoverImage { get; set; }
    public string? ThumbnailImage { get; set; }

    public IList<ArticleAttachment>? Attachments { get; set; }
}