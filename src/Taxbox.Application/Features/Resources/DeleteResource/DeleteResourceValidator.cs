using FluentValidation;

namespace Taxbox.Application.Features.Resources.DeleteResource;

public class DeleteResourceValidator : AbstractValidator<DeleteResourceRequest>
{
    public DeleteResourceValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}