using MassTransit;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Resources;

public record GetResourceResponse
{
    public ResourceId Id { get; set; }
    public string DisplayName { get; set; } = null!;
    public string FileUrl { get; set; } = null!;
    public ResourceType ResourceType { get; set; }
    public CategoryId CategoryId { get; set; }
    public Category? Category { get; init; }
}