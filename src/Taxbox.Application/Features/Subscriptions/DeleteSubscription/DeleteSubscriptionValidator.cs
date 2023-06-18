using FluentValidation;

namespace Taxbox.Application.Features.Subscriptions.DeleteSubscription;

public class DeleteSubscriptionValidator : AbstractValidator<DeleteSubscriptionRequest>
{
    public DeleteSubscriptionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}