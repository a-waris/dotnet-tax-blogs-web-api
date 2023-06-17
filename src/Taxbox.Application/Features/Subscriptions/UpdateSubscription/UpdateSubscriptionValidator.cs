using FluentValidation;
using System;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Subscriptions.UpdateSubscription;

public class UpdateSubscriptionValidator : AbstractValidator<UpdateSubscriptionRequest>
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
            .Must(x => x.Equals("days", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("months", StringComparison.OrdinalIgnoreCase) ||
                       x.Equals("years", StringComparison.OrdinalIgnoreCase));
    }
}