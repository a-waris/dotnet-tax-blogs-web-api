using FluentValidation;

namespace Taxbox.Application.Features.Articles.BulkAddOrUpdateArticles;

public class BulkAddOrUpdateArticlesValidator : AbstractValidator<BulkAddOrUpdateArticlesRequest>
{
    public BulkAddOrUpdateArticlesValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.Articles)
            .NotNull()
            .Must(x => x.Count > 0)
            .WithMessage("Articles list cannot be empty")
            .Must(x => x.Count <= 1000)
            .WithMessage("Articles list cannot be more than 1000");
    }
}