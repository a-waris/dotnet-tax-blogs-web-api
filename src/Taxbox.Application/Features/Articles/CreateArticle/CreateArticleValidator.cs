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
        
        // RuleFor(x => x.Tags)
        //     .ChildRules()
    }
}