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
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;

    public CreateAuthorHandler(IElasticSearchService<Author> esService, IS3Service s3Service,
        IOptions<AWSConfiguration> appSettings)
    {
        _esService = esService;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }


    public async Task<Result<GetAuthorResponse>> Handle(CreateAuthorRequest request,
        CancellationToken cancellationToken)
    {
        var author = request.Adapt<Author>();
        author.JoinDate = DateTime.Now.Date;

        var created = await _esService.AddOrUpdate(author);
        return created.Adapt<GetAuthorResponse>();
    }
}