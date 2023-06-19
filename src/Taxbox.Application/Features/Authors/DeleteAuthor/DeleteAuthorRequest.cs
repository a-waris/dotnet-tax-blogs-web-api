using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Authors.DeleteAuthor;

public record DeleteAuthorRequest(string Id) : IRequest<Result>;