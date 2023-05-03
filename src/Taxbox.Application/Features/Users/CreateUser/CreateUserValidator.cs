﻿using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Users.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserRequest>
{
    public CreateUserValidator(IContext context)
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(255);
        
        RuleFor(x => x.Email)
            .NotEmpty()
            .MaximumLength(254)
            .EmailAddress()
            .MustAsync(async (email, ct) => !await context.Users.AnyAsync(y => y.Email.ToLower() 
                                                                               == email.ToLower()))
            .WithMessage("A user with this email already exists.");

    }
}