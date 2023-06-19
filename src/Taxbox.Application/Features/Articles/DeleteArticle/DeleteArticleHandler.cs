using Ardalis.Result;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles.DeleteArticle;

public class DeleteArticleHandler : IRequestHandler<DeleteArticleRequest, Result>
{
    private readonly IElasticSearchService<Article> _esService;


    public DeleteArticleHandler(IElasticSearchService<Article> esService)
    {
        _esService = esService;
    }

    public async Task<Result> Handle(DeleteArticleRequest request, CancellationToken cancellationToken)
    {
        var article = await _esService.Get(request.Id.ToString()!);

        var deleted = await _esService.Remove(request.Id.ToString()!);
        if (!deleted) return Result.Error("Failed to delete article");
        return Result.Success();
    }
}