using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.BulkRemoveArticles;

public record BulkRemoveArticlesRequest : IRequest<Result<BulkRemoveArticlesResponse>>
{
    public IList<string> ArticleIds { get; set; } = null!;
}