using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles;

public record GetArticleResponse
{
    public ArticleId Id { get; init; }
    public string Title { get; set; } = null!;
    public string? Metadata { get; set; }
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime? Date { get; set; }
    public IList<string> Tags { get; set; }
}