using Ardalis.Result;
using MediatR;
using Newtonsoft.Json;
using System;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Tickets.CreateTicket;

public record CreateTicketRequest : IRequest<Result<GetTicketResponse>>
{
    public SubscriptionId SubscriptionId { get; set; }
    public string TicketType { get; set; } = null!;
    public string Status { get; set; } = "pending"; // "pending", "approved", "rejected
    public string? Metadata { get; set; }
}