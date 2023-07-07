using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Users.UpdateUser;

public record UpdateUserRequest : IRequest<Result>
{
    [JsonIgnore] public UserId Id { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }

    public IFormFile? DisplayPicture { get; init; }

    public string? MetadataJson { get; init; }

    public string? StripeCustomerToken { get; init; }
}