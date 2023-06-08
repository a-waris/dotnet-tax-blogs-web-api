using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Resources.GetResourceById;

public record GetResourceByIdRequest(ResourceId Id) : IRequest<Result<GetResourceResponse>>;