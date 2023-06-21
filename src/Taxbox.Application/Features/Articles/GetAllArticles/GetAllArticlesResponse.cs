using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public record GetAllArticlesResponse
{
    public string Id { get; init; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Title { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public Metadata? Metadata { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string Content { get; set; } = null!;    
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string HtmlContent { get; set; } = null!;

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public IList<string> AuthorIds { get; set; } = new List<string>();

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? CreatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? UpdatedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public DateTime? PublishedAt { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<string>? Tags { get; set; }

    public bool IsPublic { get; set; }

    public bool IsPublished { get; set; }
    
    public bool IsDraft { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? CoverImage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? ThumbnailImage { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<ArticleAttachment>? Attachments { get; set; }
    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Category { get; set; }    
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Slug { get; set; }
}