using MediatR;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Resources.GetAllResources;

public record GetAllResourcesRequest : PaginatedRequest, IRequest<PaginatedList<GetResourceResponse>>
{
    public string? DisplayName { get; init; }
    public ResourceType? ResourceType { get; init; }
    public CategoryId? CategoryId { get; init; }
}