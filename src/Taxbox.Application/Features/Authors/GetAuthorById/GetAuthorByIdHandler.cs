using Ardalis.Result;
using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Authors.GetAuthorById;

public class GetAuthorByIdHandler : IRequestHandler<GetAuthorByIdRequest, Result<GetAuthorResponse>>
{
    private readonly IElasticSearchService<Author> _eSservice;

    public GetAuthorByIdHandler(IElasticSearchService<Author> eSservice)
    {
        _eSservice = eSservice;
    }


    public async Task<Result<GetAuthorResponse>> Handle(GetAuthorByIdRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _eSservice.Get(request.Id.ToString()!);
        if (result.Source == null)
        {
            return Result.NotFound();
        }

        result.Source.Id = Guid.Parse(result.Id);
        return result.Source.Adapt<GetAuthorResponse>();

    }
}