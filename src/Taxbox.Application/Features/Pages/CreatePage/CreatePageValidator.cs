using FluentValidation;

namespace Taxbox.Application.Features.Pages.CreatePage;

public class CreatePageValidator : AbstractValidator<CreatePageRequest>
{
    public CreatePageValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.Label)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.HtmlContent)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.Slug)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.ParentName)
            .NotEmpty()
            .NotNull();
    }
}