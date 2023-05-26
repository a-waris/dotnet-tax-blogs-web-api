using FluentValidation;

namespace Taxbox.Application.Features.Articles.DeleteArticle;

public class DeleteArticleValidator : AbstractValidator<DeleteArticleRequest>
{
    public DeleteArticleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}