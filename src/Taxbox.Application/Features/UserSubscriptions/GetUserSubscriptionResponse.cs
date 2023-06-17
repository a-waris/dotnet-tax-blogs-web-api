using System;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.UserSubscriptions;

public record GetUserSubscriptionResponse
{
    public UserSubscriptionId Id { get; set; }
    public DateTime SubscriptionStartDate { get; set; }
    public DateTime SubscriptionEndDate { get; set; }
    public DateTime NextBillingDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool AutoRenewal { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime TrialStartDate { get; set; }
    public DateTime TrialEndDate { get; set; }
    public DateTime? CancellationDate { get; set; }

    public SubscriptionId SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    public UserId UserId { get; set; }
    public User? User { get; set; }
}