using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Tickets.GetTicketById;

public record GetTicketByIdRequest(TicketId Id) : IRequest<Result<GetTicketResponse>>;