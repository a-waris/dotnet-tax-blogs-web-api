using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Taxbox.Application.Features.Articles.BulkImportArticles;

public record BulkImportArticlesRequest : IRequest<Result<BulkImportArticlesResponse>>
{
    public IFormFile File { get; init; } = null!;
}