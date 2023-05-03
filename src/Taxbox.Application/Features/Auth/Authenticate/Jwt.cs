using System;

namespace Taxbox.Application.Features.Auth.Authenticate;

public record Jwt
{
    public string Token { get; init; } = null!;
    public DateTime ExpDate { get; init; }
}