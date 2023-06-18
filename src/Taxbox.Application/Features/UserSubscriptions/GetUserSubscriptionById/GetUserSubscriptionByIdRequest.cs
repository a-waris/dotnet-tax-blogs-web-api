using Ardalis.Result;
using MediatR;
using Taxbox.Application.Features.Subscriptions;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.UserSubscriptions.GetUserSubscriptionById;

public record GetUserSubscriptionByIdRequest
    (UserSubscriptionId Id) : IRequest<Result<GetUserSubscriptionResponse>>;