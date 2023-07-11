using MassTransit;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Domain.Entities;

[Table("Subscriptions")]
public class Subscription : Entity<SubscriptionId>
{
    public override SubscriptionId Id { get; set; } = NewId.NextGuid();
    public string Name { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int ValidityPeriod { get; set; } // 1, 2, 3, etc.
    public string ValidityPeriodType { get; set; } = null!; // Monthly, Yearly, etc.
    public decimal VAT { get; set; }
    [InverseProperty(nameof(Subscription))]
    public virtual ICollection<Ticket>? Tickets { get; set; }
}