using FluentValidation;

namespace Taxbox.Application.Features.Tickets.DeleteTicket;

public class DeleteTicketValidator : AbstractValidator<DeleteTicketRequest>
{
    public DeleteTicketValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Id is required");
    }
}