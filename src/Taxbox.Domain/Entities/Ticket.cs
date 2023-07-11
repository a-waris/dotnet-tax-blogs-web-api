using MassTransit;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Domain.Entities;

[Table("Tickets")]
public class Ticket : Entity<TicketId>
{
    public override TicketId Id { get; set; } = NewId.NextGuid();

    [ForeignKey(nameof(SubscriptionId))] public SubscriptionId SubscriptionId { get; set; }

    public Subscription? Subscription { get; init; }

    // ticket or booking
    public string TicketType { get; set; } = null!;

    // pending, approved, rejected, etc.
    public string Status { get; set; } = null!;
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}