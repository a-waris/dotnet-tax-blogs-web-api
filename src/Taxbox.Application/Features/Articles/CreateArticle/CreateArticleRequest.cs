using Ardalis.Result;
using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public record CreateArticleRequest : IRequest<Result<GetArticleResponse>>
{
    public string Title { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string Content { get; set; } = null!;
    public string Author { get; set; } = null!;
    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public IList<string>? Tags { get; set; }
}