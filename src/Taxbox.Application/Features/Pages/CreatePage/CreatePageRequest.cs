using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Pages.CreatePage;

public record CreatePageRequest : IRequest<Result<GetPageResponse>>
{
    public string Label { get; set; } = null!;
    public string HtmlContent { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string ParentName  { get; set; } = null!;
    public Metadata? Metadata { get; set; }
}