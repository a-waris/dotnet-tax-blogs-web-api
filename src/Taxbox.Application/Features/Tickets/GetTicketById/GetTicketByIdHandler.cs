using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Tickets.GetTicketById;

public class GetTicketByIdHandler : IRequestHandler<GetTicketByIdRequest, Result<GetTicketResponse>>
{
    private readonly IContext _context;


    public GetTicketByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetTicketResponse>> Handle(GetTicketByIdRequest request,
        CancellationToken cancellationToken)
    {
        var resource = await _context.Tickets.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);
        if (resource is null) return Result.NotFound();
        return resource.Adapt<GetTicketResponse>();
    }
}