using FluentValidation;
using System;

namespace Taxbox.Application.Features.UserSubscriptions.UpdateUserSubscription;

public class UpdateSubscriptionValidator : AbstractValidator<Subscriptions.UpdateSubscription.UpdateSubscriptionRequest>
{
    public UpdateSubscriptionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(254);

        RuleFor(x => x.Status)
            .NotEmpty()
            // active, inactive, etc.
            .Must(x => x.Equals("active", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("inactive", StringComparison.OrdinalIgnoreCase));

        RuleFor(x => x.Currency)
            .NotEmpty()
            // USD, AED, etc.
            .MaximumLength(100);

        RuleFor(x => x.Amount)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ValidityPeriod)
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.ValidityPeriodType)
            .NotEmpty()
            // monthly, yearly, etc.
            .Must(x => x.Equals("monthly", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("yearly", StringComparison.OrdinalIgnoreCase));
    }
}