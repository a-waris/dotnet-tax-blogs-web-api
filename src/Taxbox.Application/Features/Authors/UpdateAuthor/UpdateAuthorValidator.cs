using FluentValidation;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Authors.UpdateAuthor;

public class UpdateAuthorValidator : AbstractValidator<UpdateAuthorRequest>
{
    public UpdateAuthorValidator()
    {
        RuleFor(x => x.Name)
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.Email)
            .EmailAddress();
    }
}