﻿using Ardalis.Result;
using MediatR;
using System;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public record CreateUserSubscriptionRequest : IRequest<Result<GetUserSubscriptionResponse>>
{
    public DateTime NextBillingDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AutoRenewal { get; set; } = true;
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public DateTime TrialStartDate { get; set; }

    public DateTime? CancellationDate { get; set; }
    public SubscriptionId SubscriptionId { get; set; }
    public UserId UserId { get; set; }
}