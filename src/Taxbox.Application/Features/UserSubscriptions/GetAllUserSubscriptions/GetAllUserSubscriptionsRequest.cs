using MediatR;
using System;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Features.Subscriptions;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.UserSubscriptions.GetAllUserSubscriptions;

public record GetAllUserSubscriptionsRequest : PaginatedRequest, IRequest<PaginatedList<GetUserSubscriptionResponse>>
{
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public DateTime? NextBillingDate { get; set; }
    public bool? IsActive { get; set; }
    public bool? AutoRenewal { get; set; }
    public string? CouponCode { get; set; }
    public decimal? DiscountAmount { get; set; }
    public DateTime? CancellationDate { get; set; }
    public SubscriptionId? SubscriptionId { get; set; }
    public UserId? UserId { get; set; }
}