using Ardalis.Result;
using MediatR;
using Taxbox.Application.Features.Articles;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Authors.GetAuthorById;

public record GetAuthorByIdRequest(string Id) : IRequest<Result<GetAuthorResponse>>;