using FluentValidation;

namespace Taxbox.Application.Features.Authors.DeleteAuthor;

public class DeleteArticleValidator : AbstractValidator<Articles.DeleteArticle.DeleteArticleRequest>
{
    public DeleteArticleValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();
    }
}