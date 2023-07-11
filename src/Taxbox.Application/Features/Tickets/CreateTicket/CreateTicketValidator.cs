using FluentValidation;

namespace Taxbox.Application.Features.Tickets.CreateTicket;

public class CreateTicketValidator : AbstractValidator<CreateTicketRequest>
{
    public CreateTicketValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.SubscriptionId)
            .NotEmpty()
            .WithMessage("SubscriptionId is required");

        // ticket type must be one of the following: "ticket", "booking"

        RuleFor(x => x.TicketType)
            .NotEmpty()
            .WithMessage("TicketType is required")
            .Must(x => x is "ticket" or "booking")
            .WithMessage("TicketType must be one of the following: \"ticket\", \"booking\"");

        // status must be one of the following: "pending", "approved", "rejected"
        RuleFor(x => x.Status)
            .Must(x => x is null or "pending" or "approved" or "rejected")
            .WithMessage("Status must be one of the following: \"pending\", \"approved\", \"rejected\"");
    }
}