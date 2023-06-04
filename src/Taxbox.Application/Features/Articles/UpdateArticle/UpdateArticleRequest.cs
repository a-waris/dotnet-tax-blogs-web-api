using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.UpdateArticle;

public record UpdateArticleRequest : IRequest<Result<GetArticleResponse>>
{
    [JsonIgnore] public ArticleId Id { get; init; }
    public string? Title { get; set; }
    public Metadata? Metadata { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime? UpdatedAt { get; set; } = DateTime.Now;
    public IList<string>? Tags { get; set; }
    public bool? IsPublic { get; set; } = false;
    public IFormFile? CoverImage { get; set; }
}