using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Taxbox.Application.Features.Users.CreateUser;

public record CreateUserRequest : IRequest<Result<GetUserResponse>>
{
    public string Email { get; init; } = null!;

    public string Password { get; init; } = null!;

    public bool IsAdmin { get; init; }
    public string FirstName { get; init; } = null!;
    public string LastName { get; init; } = null!;

    public IFormFile? DisplayPicture { get; init; }
}