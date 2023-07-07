using FluentValidation;

namespace Taxbox.Application.Features.Users.UpdateUser;

public class UpdateUserValidator : AbstractValidator<UpdateUser.UpdateUserRequest>
{
    public UpdateUserValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.DisplayPicture)
            .Must(x => x == null || x.ContentType.Contains("image"))
            .WithMessage("Display picture must be an image.");
    }
}