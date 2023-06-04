using Taxbox.Domain.Entities.Common;
using MassTransit;
using Microsoft.AspNetCore.Http;

namespace Taxbox.Domain.Entities;

public class User : Entity<UserId>
{
    public override UserId Id { get; set; } = NewId.NextGuid();
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string? DisplayPicture { get; set; }
}