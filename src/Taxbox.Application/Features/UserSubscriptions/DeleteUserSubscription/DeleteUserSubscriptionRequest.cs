using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.UserSubscriptions.DeleteUserSubscription;

public record DeleteUserSubscriptionRequest(UserSubscriptionId Id) : IRequest<Result>;