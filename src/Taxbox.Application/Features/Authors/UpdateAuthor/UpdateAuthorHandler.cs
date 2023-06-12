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

namespace Taxbox.Application.Features.Authors.UpdateAuthor;

public class UpdateAuthorHandler : IRequestHandler<UpdateAuthorRequest, Result<GetAuthorResponse>>
{
    private readonly IElasticSearchService<Author> _eSservice;
    private readonly IS3Service _s3Service;
    private readonly IOptions<AWSConfiguration> _appSettings;

    public UpdateAuthorHandler(IElasticSearchService<Author> eSservice, IS3Service s3Service,
        IOptions<AWSConfiguration> appSettings)
    {
        _eSservice = eSservice;
        _s3Service = s3Service;
        _appSettings = appSettings;
    }


    public async Task<Result<GetAuthorResponse>> Handle(UpdateAuthorRequest request,
        CancellationToken cancellationToken)
    {
        var article = request.Adapt<Author>();

        var existingAuthor = await _eSservice.Get(request.Id.Adapt<string>());
        if (existingAuthor.Source == null)
        {
            return Result.NotFound();
        }

        article.JoinDate = DateTime.Now.Date;


        var result = await _eSservice.AddOrUpdate(request.Adapt(existingAuthor.Source));
        return result.Adapt<GetAuthorResponse>();
    }
}