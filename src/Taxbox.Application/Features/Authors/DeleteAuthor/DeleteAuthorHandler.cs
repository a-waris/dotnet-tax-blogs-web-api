using Ardalis.Result;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.DeleteAuthor;

public class DeleteAuthorHandler : IRequestHandler<DeleteAuthorRequest, Result>
{
    private readonly IElasticSearchService<Author> _esService;


    public DeleteAuthorHandler(IElasticSearchService<Author> esService)
    {
        _esService = esService;
    }

    public async Task<Result> Handle(DeleteAuthorRequest request, CancellationToken cancellationToken)
    {
        var author = await _esService.Get(request.Id.ToString()!);

        var deleted = await _esService.Remove(request.Id.ToString()!);
        if (!deleted) return Result.Error("Failed to delete author");
        return Result.Success();
    }
}