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

namespace Taxbox.Application.Features.Authors.GetAuthorById;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdRequest, Result<GetAuthorResponse>>
{
    private readonly IElasticSearchService<Author> _eSservice;
    private readonly IOptions<ElasticSearchConfiguration> _appSettings;

    public GetAuthorByIdHandler(IElasticSearchService<Author> eSservice,
        IOptions<ElasticSearchConfiguration> appSettings)
    {
        _eSservice = eSservice;
        _appSettings = appSettings;
    }


    public async Task<Result<GetAuthorResponse>> Handle(GetAuthorByIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _eSservice.Index(_appSettings.Value.AuthorsIndex).Get(request.Id.ToString()!);
        if (result.Source == null)
        {
            return Result.NotFound();
        }

        result.Source.Id = result.Id;
        return result.Source.Adapt<GetAuthorResponse>();

    }
}