using MassTransit;
using System.ComponentModel.DataAnnotations.Schema;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Domain.Entities;

[Table("PaymentMethods")]
public class UserPaymentMethod : Entity<UserPaymentMethodId>
{
    public override UserPaymentMethodId Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; } = null!;
    public string CardNumber { get; set; } = null!;
    public string ExpirationDate { get; set; } = null!;
    public string Cvv { get; set; } = null!;
    public string CardType { get; set; } = null!;
    public bool IsDefault { get; set; } = false;
    [ForeignKey(nameof(BillingAddressId))] public BillingAddressId BillingAddressId { get; set; }
    public BillingAddress? BillingAddress { get; set; }
    [ForeignKey(nameof(UserId))] public UserId UserId { get; set; }
    public User? User { get; set; }
}