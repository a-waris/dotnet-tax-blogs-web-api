using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.GetArticleById;

public record GetArticleByIdRequest(ArticleId Id) : IRequest<Result<GetArticleResponse>>;