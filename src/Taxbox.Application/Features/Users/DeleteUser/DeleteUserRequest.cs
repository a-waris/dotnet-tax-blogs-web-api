using Ardalis.Result;
using Taxbox.Domain.Entities.Common;
using MediatR;

namespace Taxbox.Application.Features.Users.DeleteUser;

public record DeleteUserRequest(UserId Id) : IRequest<Result>;