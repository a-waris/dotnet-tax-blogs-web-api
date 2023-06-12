using Taxbox.Application.Features.Users;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Auth.Authenticate;

public record AuthenticateResponse : Jwt
{
    public User User { get; init; } = null!;
}