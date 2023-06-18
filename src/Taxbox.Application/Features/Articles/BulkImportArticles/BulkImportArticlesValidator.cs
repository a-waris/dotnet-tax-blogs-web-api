using FluentValidation;

namespace Taxbox.Application.Features.Articles.BulkImportArticles;

public class BulkImportArticlesValidator : AbstractValidator<BulkImportArticlesRequest>
{
    private const int FileSize = 5 * 1024 * 1024; // 5MB

    public BulkImportArticlesValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        RuleFor(x => x.File)
            .NotNull()
            // check if file is excel file by checking the extension of the file
            .Must(x => x.FileName.EndsWith(".xlsx") || x.FileName.EndsWith(".xls"))
            .WithMessage("Invalid file type")
            .Must(x => x.Length < FileSize)
            .WithMessage("File size should be less than 5MB");
    }
}