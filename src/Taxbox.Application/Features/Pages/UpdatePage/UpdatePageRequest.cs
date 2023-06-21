using Ardalis.Result;
using MassTransit;
using MediatR;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Pages.UpdatePage;

public record UpdatePageRequest : IRequest<Result<GetPageResponse>>
{
    [JsonIgnore] public PageId Id { get; set; } = NewId.NextGuid();
    public string? Label { get; set; } = null!;
    public string? HtmlContent { get; set; } = null!;
    public string? Slug { get; set; } = null!;
    public string? ParentName { get; set; } = null!;
    public Metadata? Metadata { get; set; }
}