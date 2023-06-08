using FluentValidation;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Resources.UpdateResource;

public class UpdateResourceValidator : AbstractValidator<UpdateResourceRequest>
{
    public UpdateResourceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.DisplayName)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.ResourceType)
            .IsInEnum();

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .NotNull();
    }
}