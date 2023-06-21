using FluentValidation;
using System.Collections.Generic;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public class GetAllArticlesValidator : AbstractValidator<GetAllArticlesRequestBase>
{
    public GetAllArticlesValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;
        var allowedFields = new List<string>
        {
            "Title",
            "CreatedAt",
            "UpdatedAt",
            "PublishedAt",
            "IsPublished",
            "IsDraft",
            "IsPublic",
            "Slug",
            "Category"
        };

        RuleFor(x => x.SortBy)
            .Must(x => x == null || allowedFields.Contains(x))
            .WithMessage($"SortBy must be one of the following: {string.Join(", ", allowedFields)}");
    }
}