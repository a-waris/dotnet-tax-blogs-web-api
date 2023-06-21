using FluentValidation;

namespace Taxbox.Application.Features.Pages.UpdatePage;

public class UpdatePageValidator : AbstractValidator<UpdatePageRequest>
{
    public UpdatePageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
        RuleFor(x => x.Label)
            .NotEmpty();
        RuleFor(x => x.Slug)
            .NotEmpty();
        RuleFor(x => x.ParentName)
            .NotEmpty();
        RuleFor(x => x.HtmlContent)
            .NotEmpty();
    }
}