using MassTransit;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Domain.Entities;

[Table("UserSubscriptions")]
public class UserSubscription : Entity<UserSubscriptionId>
{
    public override UserSubscriptionId Id { get; set; } = NewId.NextGuid();


    public bool IsActive { get; set; } = true;
    public bool AutoRenewal { get; set; }
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public DateTime? CancellationDate { get; set; }

    [ForeignKey(nameof(SubscriptionId))] public SubscriptionId SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    [ForeignKey(nameof(UserId))] public UserId UserId { get; set; }
    public User? User { get; set; }
    public string? CustomerId { get; set; }
}