using MediatR;
using System;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Features.Resources;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Tickets.GetAllTickets;

public record GetAllTicketsRequest : PaginatedRequest, IRequest<PaginatedList<GetTicketResponse>>
{
    public SubscriptionId? SubscriptionId { get; set; }
    public string? TicketType { get; set; }
    public string? Status { get; set; }
    public string? Metadata { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}