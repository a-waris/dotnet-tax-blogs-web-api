using Ardalis.Result;
using MediatR;
using Taxbox.Application.Features.Categories;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Subscriptions.GetSubscriptionById;

public record GetSubscriptionByIdRequest
    (SubscriptionId Id) : IRequest<Result<GetSubscriptionResponse>>;