using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Domain.Entities;

[Table("BillingAddresses")]
public class BillingAddress : Entity<BillingAddressId>
{
    public override BillingAddressId Id { get; set; } = NewId.NextGuid();
    public string Address1 { get; set; } = null!;
    public string? Address2 { get; set; }
    public string City { get; set; } = null!;
    public string State { get; set; } = null!;
    public string PostalCode { get; set; } = null!;
    public string Country { get; set; } = null!;
    [ForeignKey(nameof(UserId))] public UserId UserId { get; set; }
    public User? User { get; init; }
}