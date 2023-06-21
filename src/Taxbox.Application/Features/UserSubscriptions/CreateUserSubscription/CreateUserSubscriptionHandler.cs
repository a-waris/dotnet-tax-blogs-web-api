using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;
using Taxbox.Domain.PaymentGateway.Interfaces;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public class
    CreateUserSubscriptionHandler : IRequestHandler<CreateUserSubscriptionRequest, Result<GetUserSubscriptionResponse>>
{
    private readonly IContext _context;
    private readonly IStripeService _stripeService;


    public CreateUserSubscriptionHandler(IContext context, IStripeService stripeService)
    {
        _context = context;
        _stripeService = stripeService;
    }

    public async Task<Result<GetUserSubscriptionResponse>> Handle(CreateUserSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<UserSubscription>();
        created.SubscriptionStartDate = DateTime.UtcNow;

        // if (request.SubscriptionStartDate is not null)
        // {
            var sub = await _context.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.SubscriptionId,
                cancellationToken);
            if (sub == null)
                return Result.NotFound("Subscription not found");
            SetSubscriptionDates(created, created.SubscriptionStartDate.Value, sub.ValidityPeriodType,
                sub.ValidityPeriod);
            // await HandleCouponCode(request, created);
            // await HandleDiscount(request);
            var user = await _context.Users.FindAsync(new object?[] { request.UserId },
                cancellationToken: cancellationToken);
            if (user == null)
                return Result.NotFound("User not found");
            await HandlePaymentFlow(request, user, created, sub, cancellationToken);
        // }

        _context.UserSubscriptions.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetUserSubscriptionResponse>();
    }

    private static void SetSubscriptionDates(UserSubscription created, DateTime subscriptionStartDate,
        string validityPeriodType, int validityPeriod)
    {
        created.SubscriptionStartDate = subscriptionStartDate.ToUniversalTime();
        created.SubscriptionEndDate = validityPeriodType.ToLower() switch
        {
            "days" => created.SubscriptionStartDate.Value.AddDays(validityPeriod),
            "months" => created.SubscriptionStartDate.Value.AddMonths(validityPeriod),
            _ => created.SubscriptionEndDate
        };
        created.NextBillingDate = created.SubscriptionEndDate?.AddDays(1);
    }

    // private async Task HandleCouponCode(CreateUserSubscriptionRequest request, UserSubscription created)
    // {
    //     if (created.CouponCode != default)
    //     {
    //         // TODO: Handle coupon code
    //     }
    //     
    // }
    //
    // private async Task HandleDiscount(CreateUserSubscriptionRequest request)
    // {
    //     if (request.DiscountAmount != default)
    //     {
    //         // TODO: Handle discount
    //     }
    // }

    private async Task HandlePaymentFlow(CreateUserSubscriptionRequest request,
        User user, UserSubscription created, Subscription sub, CancellationToken cancellationToken)
    {
        try
        {
            if (request.CardDetails == null)
                throw new Exception("Card details not provided");

            var createCustomerResource =
                new CreateCustomerResource(user.Email, $"{user.FirstName} {user.LastName}", request.CardDetails);
            var customer = await _stripeService.CreateCustomer(createCustomerResource, cancellationToken);

            // set customer id
            created.CustomerId = customer.CustomerId;

            await _stripeService.CreateCharge(new CreateChargeResource(
                Currency: sub.Currency,
                Amount: (long)(sub.Amount * 100),
                Description: "Subscription charge for " + sub.Name,
                CustomerId: customer.CustomerId,
                ReceiptEmail: user.Email), cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}