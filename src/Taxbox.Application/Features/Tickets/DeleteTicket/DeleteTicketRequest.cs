using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Tickets.DeleteTicket;

public record DeleteTicketRequest(TicketId Id) : IRequest<Result>;