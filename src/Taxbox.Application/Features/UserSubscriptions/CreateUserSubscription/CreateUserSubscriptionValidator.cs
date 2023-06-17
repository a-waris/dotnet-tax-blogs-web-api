using FluentValidation;
using System;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public class CreateUserSubscriptionValidator : AbstractValidator<CreateUserSubscriptionRequest>
{
    public CreateUserSubscriptionValidator()
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
    }
}