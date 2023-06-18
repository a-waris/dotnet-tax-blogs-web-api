using FluentValidation;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Resources.CreateResource;

public class CreateCategoryValidator : AbstractValidator<CreateResourceRequest>
{
    public CreateCategoryValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.File)
            .NotEmpty()
            .NotNull();

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .NotNull();
    }
}