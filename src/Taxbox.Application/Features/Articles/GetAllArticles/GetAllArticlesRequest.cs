using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public record GetAllArticlesRequest : PaginatedRequest, IRequest<PaginatedList<GetArticleResponse>>
{
    public string? Title { get; set; }
    public Metadata? Metadata { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IList<string>? Tags { get; set; }

    public override string ToString()
    {
        return $"{Title} {Metadata} {Content} {Author} {CreatedAt} {UpdatedAt} {Tags}";
    }
}