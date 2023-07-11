using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Tickets.DeleteTicket;

public class DeleteTicketHandler : IRequestHandler<DeleteTicketRequest, Result>
{
    private readonly IContext _context;

    public DeleteTicketHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteTicketRequest request, CancellationToken cancellationToken)
    {
        var resource = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (resource is null) return Result.NotFound();
        _context.Tickets.Remove(resource);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}