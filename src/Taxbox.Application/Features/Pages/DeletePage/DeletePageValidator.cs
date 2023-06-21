using FluentValidation;

namespace Taxbox.Application.Features.Pages.DeletePage;

public class DeletePageValidator : AbstractValidator<DeletePageRequest>
{
    public DeletePageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}