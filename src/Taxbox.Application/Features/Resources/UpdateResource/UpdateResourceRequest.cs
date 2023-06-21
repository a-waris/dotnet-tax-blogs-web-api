using Ardalis.Result;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Resources.UpdateResource;

public record UpdateResourceRequest : IRequest<Result<GetResourceResponse>>
{
    [JsonIgnore] public ResourceId Id { get; set; } = NewId.NextGuid();
    public string? DisplayName { get; set; } = null!;
    public IFormFile? File { get; set; } = null!;
    public ResourceType? ResourceType { get; set; }
    public CategoryId? CategoryId { get; set; }
}