using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Options;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.DeleteAuthor;

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorRequest, Result>
{
    private readonly IElasticSearchService<Author> _esService;
    private readonly IOptions<ElasticSearchConfiguration> _appSettings;


    public DeleteAuthorHandler(IElasticSearchService<Author> esService,
        IOptions<ElasticSearchConfiguration> appSettings)
    {
        _esService = esService;
        _appSettings = appSettings;
    }

    public async Task<Result> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
    {
        var author = await _esService.Index("authors").Get(request.Id.ToString()!);

        var deleted = await _esService.Index(_appSettings.Value.AuthorsIndex).Remove(request.Id.ToString()!);
        if (!deleted) return Result.Error("Failed to delete author");
        return Result.Success();
    }
}