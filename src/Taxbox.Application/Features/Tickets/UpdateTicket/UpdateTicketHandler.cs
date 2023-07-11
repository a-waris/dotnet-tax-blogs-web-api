using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Tickets.UpdateTicket;

public class UpdateTicketHandler : IRequestHandler<UpdateTicketRequest, Result<GetTicketResponse>>
{
    private readonly IContext _context;


    public UpdateTicketHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetTicketResponse>> Handle(UpdateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var originalTicket = await _context.Tickets
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (originalTicket == null) return Result.NotFound();
        
        originalTicket.UpdatedAt = DateTime.UtcNow;

        if (request.SubscriptionId != null)
        {
            originalTicket.SubscriptionId = (SubscriptionId)request.SubscriptionId;
        }

        if (!string.IsNullOrEmpty(request.Status))
        {
            originalTicket.Status = request.Status;
        }

        if (!string.IsNullOrEmpty(request.Metadata))
        {
            originalTicket.Metadata = request.Metadata;
        }

        if (!string.IsNullOrEmpty(request.TicketType))
        {
            originalTicket.TicketType = request.TicketType;
        }

        _context.Tickets.Update(originalTicket);
        await _context.SaveChangesAsync(cancellationToken);
        return originalTicket.Adapt<GetTicketResponse>();
    }
}