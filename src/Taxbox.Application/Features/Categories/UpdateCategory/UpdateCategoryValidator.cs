using FluentValidation;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Categories.UpdateCategory;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.Name)
            .MaximumLength(StringSizes.Max);
    }
}