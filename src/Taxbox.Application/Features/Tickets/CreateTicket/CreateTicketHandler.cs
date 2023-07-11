using Ardalis.Result;
using Mapster;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Tickets.CreateTicket;

public class CreateTicketHandler : IRequestHandler<CreateTicketRequest, Result<GetTicketResponse>>
{
    private readonly IContext _context;


    public CreateTicketHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetTicketResponse>> Handle(CreateTicketRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Ticket>();
        created.CreatedAt = DateTime.UtcNow;
        _context.Tickets.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetTicketResponse>();
    }
}