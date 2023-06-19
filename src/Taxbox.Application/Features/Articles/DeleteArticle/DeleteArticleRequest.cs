using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.DeleteArticle;

public record DeleteArticleRequest(string Id) : IRequest<Result>;