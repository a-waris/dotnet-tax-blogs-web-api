using FluentValidation;

namespace Taxbox.Application.Features.Pages.UpdatePage;

public class UpdatePageValidator : AbstractValidator<UpdatePageRequest>
{
    public UpdatePageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}