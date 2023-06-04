using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Users;

public record GetUserResponse
{
    public UserId Id { get; init; }

    public string Email { get; init; } = null!;
    public bool IsAdmin { get; init; }

    public string FirstName { get; init; } = null!;

    public string LastName { get; init; } = null!;

    public string? DisplayPicture { get; init; }
}