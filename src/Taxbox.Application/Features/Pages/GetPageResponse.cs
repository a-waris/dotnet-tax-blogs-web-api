using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;


namespace Taxbox.Application.Features.Pages;

public record GetPageResponse
{
    public PageId Id { get; set; }
    public string Label { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string? Slug { get; set; }
    public string? ParentName  { get; set; }

}