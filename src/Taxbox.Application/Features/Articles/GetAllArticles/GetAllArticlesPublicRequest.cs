using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public record GetAllArticlesPublicRequest : PaginatedRequest, IRequest<PaginatedList<GetAllArticlesResponse>>
{
    private const bool IsPublic = true;
    public string? Title { get; set; }
    public Metadata? Metadata { get; set; }
    public string? Content { get; set; }
    public IList<string>? AuthorIds { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IList<string>? Tags { get; set; }
    public bool? IsPublished { get; set; }
    public string? SourceFields { get; set; }
    public string? FreeTextSearch { get; set; }

    public override string ToString()
    {
        return
            $"{CurrentPage} {PageSize} {CurrentPage} {PageSize} {Title} {Metadata} {Content} {AuthorIds} {CreatedAt} {UpdatedAt} {Tags} {IsPublic} {IsPublished} {SourceFields}";
    }
}