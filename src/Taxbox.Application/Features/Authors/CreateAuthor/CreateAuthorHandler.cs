using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.CreateAuthor;

public class CreateAuthorHandler : IRequestHandler<CreateAuthorRequest, Result<GetAuthorResponse>>
{
    private readonly IElasticSearchService<Author> _esService;
    private readonly IOptions<ElasticSearchConfiguration> _appSettings;

    public CreateAuthorHandler(IElasticSearchService<Author> esService,
        IOptions<ElasticSearchConfiguration> appSettings)
    {
        _esService = esService;
        _appSettings = appSettings;
    }


    public async Task<Result<GetAuthorResponse>> Handle(CreateAuthorRequest request,
        CancellationToken cancellationToken)
    {
        var author = request.Adapt<Author>();
        author.JoinDate = DateTime.UtcNow;

        var created = await _esService.Index(_appSettings.Value.AuthorsIndex).AddOrUpdate(author);
        return created.Adapt<GetAuthorResponse>();
    }
}