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
            .NotEmpty();

        RuleFor(x => x.Author)
            .NotEmpty()
            .MaximumLength(StringSizes.Max);

        RuleFor(x => x.CoverImage)
            .Must(x => x == null || x.ContentType.Contains("image"))
            .WithMessage("CoverImage must be an image.");

        RuleFor(x => x.ThumbnailImage)
            .Must(x => x == null || x.ContentType.Contains("image"))
            .WithMessage("ThumbnailImage must be an image.");


        RuleFor(x => x.Attachments)
            .Must(x => x == null || x.Count <= 5)
            .WithMessage("Attachments must be less than or equal to 5.");
    }
}