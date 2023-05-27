using FluentValidation;
using System.Collections.Generic;
using System.Linq;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Articles.UpdateArticle;

public class UpdateArticleValidator : AbstractValidator<UpdateArticleRequest>
{
    public UpdateArticleValidator()
    {
        // RuleFor(x => x.Title)
        //     .MaximumLength(StringSizes.Max);

        RuleFor(x => x.Metadata)
            .ChildRules(metadata =>
            {
                metadata.RuleFor(x => x!.Category)
                    .MaximumLength(StringSizes.Max);

                metadata.RuleFor(x => x!.Language)
                    .MaximumLength(StringSizes.Max);

                metadata.RuleFor(x => x!.Views)
                    .GreaterThan(0);
            });


        // RuleFor(x => x.Content)
        //     .MaximumLength(StringSizes.Max);
        //
        // RuleFor(x => x.Author)
        //     .MaximumLength(StringSizes.Max);

        RuleFor(x => x.UpdatedAt)
            .NotNull();

        RuleFor(x => x.Tags)
            .Must(x => x != null && x.Count == x.Distinct().Count())
            .WithMessage("Tags must be unique");
    }
}