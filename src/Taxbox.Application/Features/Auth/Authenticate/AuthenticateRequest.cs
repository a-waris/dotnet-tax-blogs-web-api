using Ardalis.Result;
using Taxbox.Application.Common.Responses;
using MediatR;

namespace Taxbox.Application.Features.Auth.Authenticate;

public record AuthenticateRequest : IRequest<Result<Jwt>>
{
    public string Email { get; init; } = null!;

    public string Password { get; init; }  = null!;
}