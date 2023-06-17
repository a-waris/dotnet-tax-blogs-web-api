using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Taxbox.Application.Features.Articles.BulkAddArticles;

public record BulkAddArticlesRequest : IRequest<Result<BulkAddArticlesResponse>>
{
    public IFormFile File { get; init; } = null!;
}