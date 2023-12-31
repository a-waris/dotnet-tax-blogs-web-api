﻿using Ardalis.Result;
using MediatR;
using System;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public record CreateUserSubscriptionRequest : IRequest<Result<GetUserSubscriptionResponse>>
{
    public bool AutoRenewal { get; set; } = true;
    public SubscriptionId SubscriptionId { get; set; }
    public UserId UserId { get; set; }

    public string CustomerId { get; set; } = null!;
}