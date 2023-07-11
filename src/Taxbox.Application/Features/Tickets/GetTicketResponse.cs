using System;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Tickets;

public record GetTicketResponse
{
    public TicketId Id { get; set; }
    public SubscriptionId SubscriptionId { get; set; }
    public Subscription? Subscription { get; set; }
    public string TicketType { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}