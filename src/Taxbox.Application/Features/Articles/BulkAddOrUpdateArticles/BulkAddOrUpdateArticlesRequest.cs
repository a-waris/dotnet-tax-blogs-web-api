using Ardalis.Result;
using MediatR;
using System.Collections.Generic;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.BulkAddOrUpdateArticles;

public record BulkAddOrUpdateArticlesRequest : IRequest<Result<BulkAddOrUpdateArticlesResponse>>
{
    public IList<Article> Articles { get; set; } = null!;
}