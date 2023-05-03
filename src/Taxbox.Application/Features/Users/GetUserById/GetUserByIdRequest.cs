using Ardalis.Result;
using Taxbox.Domain.Entities.Common;
using MediatR;

namespace Taxbox.Application.Features.Users.GetUserById;

public record GetUserByIdRequest(UserId Id) : IRequest<Result<GetUserResponse>>;