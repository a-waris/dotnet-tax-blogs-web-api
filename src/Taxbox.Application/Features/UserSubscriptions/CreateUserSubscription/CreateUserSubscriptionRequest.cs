using Ardalis.Result;
using MediatR;
using System;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public record CreateUserSubscriptionRequest : IRequest<Result<GetUserSubscriptionResponse>>
{
    public bool AutoRenewal { get; set; } = true;
    public string? CouponCode { get; set; }
    public decimal DiscountAmount { get; set; }
    // public DateTime? SubscriptionStartDate { get; set; }
    public SubscriptionId SubscriptionId { get; set; }
    public UserId UserId { get; set; }
    
    //TODO: perhaps expose a token from the client and use that to create a charge
    public CreateCardResource? CardDetails { get; set; } = null!;
}