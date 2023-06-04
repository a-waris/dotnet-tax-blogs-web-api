using FluentValidation;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Articles.CreateArticle;

public class CreateArticleValidator : AbstractValidator<CreateArticleRequest>
{
    public CreateArticleValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.Content)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.Author)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.CoverImage)
            .Must(x => x == null || x.ContentType.Contains("image"))
            .WithMessage("CoverImage must be an image.");


        RuleFor(x => x.Attachments)
            //TODO: check for attachment type?
            .Must(x => x is not { Count: > 5 })
            .WithMessage("Attachments must be less than or equal to 5.");
    }
}