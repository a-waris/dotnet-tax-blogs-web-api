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
                    .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive == true, cancellationToken);
                return existing == null;
            })
            .WithMessage("User already has a subscription");

        RuleFor(x => x.CustomerId)
            .NotNull()
            .NotEmpty();
    }
}