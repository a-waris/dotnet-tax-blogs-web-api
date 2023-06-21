using System;
using System.Collections.Generic;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.GetAllArticles;


public static class GetAllArticlesRequestConstants {
    public const string Ascending = "asc";
    public const string Descending = "desc";
    public const string DefaultSortBy = "UpdatedAt";
}

public record GetAllArticlesRequestBase
{
    public virtual bool? IsPublic { get; set; }
    public string? Title { get; set; }
    public Metadata? Metadata { get; set; }
    public string? Content { get; set; }
    public IList<string>? AuthorIds { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? PublishedAt { get; set; }
    public IList<string>? Tags { get; set; }
    public bool? IsPublished { get; set; }
    public bool? IsDraft { get; set; }
    public string? SourceFields { get; set; }
    public string? FreeTextSearch { get; set; }
    public string? SortBy { get; set; }
    public string? SortOrder { get; set; }
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Category { get; set; }
    public string? Slug { get; set; }

    public override string ToString()
    {
        return
            $"{Title} {Metadata} {Content} {AuthorIds} {CreatedAt} {UpdatedAt} {Tags} {IsPublished} {SourceFields} {FreeTextSearch} {IsDraft} {PublishedAt}";
    }

}