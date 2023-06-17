using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
    private readonly IConfiguration _configuration;


    public CreateUserSubscriptionHandler(IContext context, IStripeService stripeService, IConfiguration configuration)
    {
        _context = context;
        _stripeService = stripeService;
        _configuration = configuration;
    }

    public async Task<Result<GetUserSubscriptionResponse>> Handle(CreateUserSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<UserSubscription>();

        // get subscription plan
        var sub = await _context.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.SubscriptionId,
            cancellationToken);

        if (sub == null) return Result.NotFound("Subscription not found");


        // if trial start date is set then set trial end date based on the subscription
        if (request.TrialStartDate is not null)
        {
            // save as UTC
            created.TrialStartDate = request.TrialStartDate.Value.ToUniversalTime();
            created.TrialEndDate = sub.ValidityPeriodType.ToLower() switch
            {
                "days" => created.TrialStartDate.Value.AddDays(sub.ValidityPeriod),
                "months" => created.TrialStartDate.Value.AddMonths(sub.ValidityPeriod),
                _ => created.TrialEndDate
            };
            // Lets not set NextBillingDate for trial subscriptions
        }
        // else if subscription start date is set then set subscription end date based on the subscription plan
        else if (request.SubscriptionStartDate is not null)
        {
            // save as UTC
            created.SubscriptionStartDate = request.SubscriptionStartDate.Value.ToUniversalTime();
            created.SubscriptionEndDate = sub.ValidityPeriodType.ToLower() switch
            {
                "days" => created.SubscriptionStartDate.Value.AddDays(sub.ValidityPeriod),
                "months" => created.SubscriptionStartDate.Value.AddMonths(sub.ValidityPeriod),
                _ => created.SubscriptionEndDate
            };
            // set NextBillingDate to SubscriptionEndDate + 1 day
            created.NextBillingDate = created.SubscriptionEndDate?.AddDays(1);

            // TODO: handle coupon code
            // if coupon code is set
            if (created.CouponCode != default)
            {
            }

            // TODO: handle discount
            // if discount is set 
            if (request.DiscountAmount != default)
            {
            }

            // get the user 
            var user = await _context.Users.FindAsync(request.UserId);

            // if user is not found then return error
            if (user == null) return Result.NotFound("User not found");


            await HandlePaymentFlow(request, cancellationToken, user, created, sub);
        }

        _context.UserSubscriptions.Add(created);

        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetUserSubscriptionResponse>();
    }

    private async Task HandlePaymentFlow(CreateUserSubscriptionRequest request,
        CancellationToken cancellationToken,
        User user, UserSubscription created, Subscription sub)
    {
        try
        {
            // handle stripe subscription
            if (request.CardDetails == null)
            {
                return;
            }

            var createCustomerResource = new CreateCustomerResource(
                user.Email, $"{user.FirstName} {user.LastName}", request.CardDetails
            );

            var customer = await _stripeService.CreateCustomer(
                createCustomerResource, cancellationToken
            );

            // set stripe customer id
            created.CustomerId = customer.CustomerId;

            if (_configuration.GetSection("StripeOptions")["Mode"] == "live")
            {
                // create charge
                var charge = await _stripeService.CreateCharge(
                    new CreateChargeResource(
                        Currency: sub.Currency,
                        // parse into long and multiply by 100 to convert to cents
                        Amount: (long)(sub.Amount * 100),
                        Description: "Subscription charge for " + sub.Name,
                        CustomerId: customer.CustomerId,
                        ReceiptEmail: user.Email
                    ), cancellationToken
                );
            }


            var intent = await _stripeService.CreatePaymentIntent(
                new CreatePaymentIntentResource(
                    Currency: sub.Currency,
                    // parse into long and multiply by 100 to convert to cents
                    Amount: (long)(sub.Amount * 100),
                    Description: "Subscription charge for " + sub.Name,
                    ReceiptEmail: user.Email
                ),
                cancellationToken
            );
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}