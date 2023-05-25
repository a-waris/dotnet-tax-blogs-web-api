using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public record CreateArticleRequest : IRequest<Result<GetArticleResponse>>
{
    public string Title { get; set; } = null!;
    public string? Metadata { get; set; }
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime? Date { get; set; }
    public IList<string>? Tags { get; set; }
}