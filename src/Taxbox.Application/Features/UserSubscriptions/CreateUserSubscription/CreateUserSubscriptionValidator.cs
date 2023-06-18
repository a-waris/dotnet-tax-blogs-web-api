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

        // check if subscription start date is set then card details are required
        RuleFor(x => x.CardDetails)
            .NotNull()
            .WithMessage("Card details are required for subscription start date");
    }
}