using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Users.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(x => x.FirstName)
            .MaximumLength(254);

        RuleFor(x => x.LastName)
            .MaximumLength(254);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(255);

        RuleFor(x => x.DisplayPicture)
            .Must(x => x == null || x.ContentType.Contains("image"))
            .WithMessage("Display picture must be an image.");

        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(254)
            .EmailAddress()
            .MustAsync(async (email, ct) => !await context.Users.AnyAsync(y =>
                string.Equals(y.Email, email), cancellationToken: ct))
            .WithMessage("A user with this email already exists.");
    }
}