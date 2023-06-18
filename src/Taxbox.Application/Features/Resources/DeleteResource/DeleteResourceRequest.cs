using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Resources.DeleteResource;

public record DeleteResourceRequest(ResourceId Id) : IRequest<Result>;