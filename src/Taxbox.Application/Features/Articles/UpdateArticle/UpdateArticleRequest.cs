using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Taxbox.Application.Features.Articles.CreateArticle;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.UpdateArticle;

public record UpdateArticleRequest : IRequest<Result<GetArticleResponse>>
{
    [JsonIgnore] public string Id { get; init; } = null!;
    public string? Title { get; set; }
    public Metadata? Metadata { get; set; }
    public string? HtmlContent { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public IList<string>? Tags { get; set; }
    public bool? IsPublic { get; set; } = false;
    public bool? IsPublished { get; set; } = false;
    public bool? IsDraft { get; set; } = false;
    public DateTime? PublishedAt { get; set; }

    public IList<string>? AuthorIds { get; set; } = new List<string>();
    public IFormFile? CoverImage { get; set; }
    public IFormFile? ThumbnailImage { get; set; }
    public IList<ArticleAttachmentRequest>? Attachments { get; set; }

    public string? Category { get; set; }
    public string? Slug { get; set; }
}