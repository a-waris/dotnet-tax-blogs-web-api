using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Resources.CreateResource;

public record CreateResourceRequest : IRequest<Result<GetResourceResponse>>
{
    public string DisplayName { get; init; } = null!;
    public IFormFile File { get; init; } = null!;
    // public ResourceType ResourceType { get; init; }
    public CategoryId CategoryId { get; init; }
}