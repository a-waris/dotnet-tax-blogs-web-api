using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public class CreateUserSubscriptionValidator : AbstractValidator<CreateUserSubscriptionRequest>
{
    public CreateUserSubscriptionValidator(IContext context)
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.SubscriptionId)
            .NotNull();

        RuleFor(x => x.UserId)
            .NotNull();

        // pass SubscriptionStartDate or TrialStartDate but not both
        RuleFor(x => x.SubscriptionStartDate)
            .Must((request, startDate) => startDate == default || request.TrialStartDate == default)
            .WithMessage("SubscriptionStartDate and TrialStartDate cannot be passed together");

        // // check if user exists
        // RuleFor(x => x.UserId)
        //     .MustAsync(async (request, userId, cancellationToken) =>
        //     {
        //         var user = await context.Users
        //             .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
        //         return user != null;
        //     })
        //     .WithMessage("User not found");

        // // check if subscription exists
        // RuleFor(x => x.SubscriptionId)
        //     .MustAsync(async (request, subscriptionId, cancellationToken) =>
        //     {
        //         var subscription = await context.Subscriptions
        //             .FirstOrDefaultAsync(x => x.Id == subscriptionId, cancellationToken);
        //         return subscription != null;
        //     })
        //     .WithMessage("Subscription not found");

        // check if user already has a subscription
        RuleFor(x => x.UserId)
            .MustAsync(async (request, userId, cancellationToken) =>
            {
                var existing = await context.UserSubscriptions
                    .FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
                return existing == null;
            })
            .WithMessage("User already has a subscription");


        // check if subscription start date is in the future - only compare date part
        RuleFor(x => x.SubscriptionStartDate)
            .Must((request, startDate) => startDate == default || startDate > DateTime.UtcNow.Date)
            .WithMessage("SubscriptionStartDate cannot be in the past");

        // check if trial start date is in the future
        RuleFor(x => x.TrialStartDate)
            .Must((request, startDate) => startDate == default || startDate > DateTime.UtcNow.Date)
            .WithMessage("TrialStartDate cannot be in the past");

        // check if subscription start date is set then card details are required
        RuleFor(x => x.SubscriptionStartDate)
            .Must((request, startDate) => startDate == default || request.CardDetails != null)
            .WithMessage("Card details are required for subscription start date");
    }
}