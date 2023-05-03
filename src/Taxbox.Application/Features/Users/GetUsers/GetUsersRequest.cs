using MediatR;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;

namespace Taxbox.Application.Features.Users.GetUsers;

public record GetUsersRequest : PaginatedRequest, IRequest<PaginatedList<GetUserResponse>>
{
    public string? Email { get; init; }
    public bool IsAdmin { get; init; }
}