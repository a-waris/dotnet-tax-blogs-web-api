using Ardalis.Result;
using MediatR;
using System;
using System.Text.Json.Serialization;
using Taxbox.Application.Features.Subscriptions;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.UserSubscriptions.UpdateUserSubscription;

public record UpdateUserSubscriptionRequest : IRequest<Result<GetUserSubscriptionResponse>>
{
    [JsonIgnore] public UserSubscriptionId Id { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    // public DateTime NextBillingDate { get; set; }
    public bool? IsActive { get; set; } = true;
    public bool? AutoRenewal { get; set; }
    // public string? CouponCode { get; set; }
    // public decimal? DiscountAmount { get; set; }
    public DateTime? TrialStartDate { get; set; }
    public DateTime? TrialEndDate { get; set; }
    public DateTime? CancellationDate { get; set; }

    public SubscriptionId SubscriptionId { get; set; }
    // public UserId UserId { get; set; }
}