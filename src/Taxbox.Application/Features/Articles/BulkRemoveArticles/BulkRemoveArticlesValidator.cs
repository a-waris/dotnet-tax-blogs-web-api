using FluentValidation;

namespace Taxbox.Application.Features.Articles.BulkRemoveArticles;

public class BulkRemoveArticlesValidator : AbstractValidator<BulkRemoveArticles.BulkRemoveArticlesRequest>
{
    public BulkRemoveArticlesValidator()
    {
        RuleLevelCascadeMode = ClassLevelCascadeMode;

        // RuleFor(x => x.File)
        //     .NotNull()
        //     // check if file is excel file by checking the extension of the file
        //     .Must(x => x.FileName.EndsWith(".xlsx") || x.FileName.EndsWith(".xls"))
        //     .WithMessage("Invalid file type")
        //     // check file size is less than 5MB
        //     .Must(x => x.Length < FileSize)
        //     .WithMessage("File size should be less than 2MB");
    }
}