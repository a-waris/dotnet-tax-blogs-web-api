using FluentValidation;
using Taxbox.Application.Features.Subscriptions.DeleteSubscription;

namespace Taxbox.Application.Features.UserSubscriptions.DeleteUserSubscription;

public class DeleteUserSubscriptionValidator : AbstractValidator<DeleteUserSubscriptionRequest>
{
    public DeleteUserSubscriptionValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}