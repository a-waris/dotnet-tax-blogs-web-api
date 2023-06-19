using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public record CreateArticleRequest : IRequest<Result<GetArticleResponse>>
{
    public string Title { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string? HtmlContent { get; set; } = null!;
    public string? Content { get; set; } = null!;
    public IList<string> AuthorIds { get; set; } = new List<string>();
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; } = DateTime.UtcNow;
    public IList<string>? Tags { get; set; }
    public bool IsPublic { get; set; } = false;
    public bool IsPublished { get; set; } = false;
    public IFormFile? CoverImage { get; set; }
    public IFormFile? ThumbnailImage { get; set; }
    public IList<ArticleAttachmentRequest>? Attachments { get; set; }
}

public record ArticleAttachmentRequest
{
    public IFormFile File { get; set; } = null!;
    public string Type { get; set; } = null!;
}

public enum AttachmentType
{
}