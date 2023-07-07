using Taxbox.Domain.Entities.Common;
using MassTransit;

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
    
    // TODO: need to deserialize this to an object?
    public string? MetadataJson { get; set; }
    public string? StripeCustomerToken { get; set; }
}