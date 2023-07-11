using Ardalis.Result;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Text.Json.Serialization;
using Taxbox.Application.Features.Resources;
using Taxbox.Domain.Entities.Common;
using Taxbox.Domain.Entities.Enums;

namespace Taxbox.Application.Features.Tickets.UpdateTicket;

public record UpdateTicketRequest : IRequest<Result<GetTicketResponse>>
{
    [JsonIgnore] public TicketId Id { get; set; }
    public SubscriptionId? SubscriptionId { get; set; }
    public string? TicketType { get; set; }
    public string? Status { get; set; }
    public string? Metadata { get; set; }
}