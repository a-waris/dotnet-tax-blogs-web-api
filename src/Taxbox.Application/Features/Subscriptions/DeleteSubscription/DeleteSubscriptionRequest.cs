using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Subscriptions.DeleteSubscription;

public record DeleteSubscriptionRequest(SubscriptionId Id) : IRequest<Result>;