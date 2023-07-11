using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Extensions;

namespace Taxbox.Application.Features.Tickets.GetAllTickets;

public class GetAllTicketHandler : IRequestHandler<GetAllTicketsRequest, PaginatedList<GetTicketResponse>>
{
    private readonly IContext _context;

    public GetAllTicketHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetTicketResponse>> Handle(GetAllTicketsRequest request,
        CancellationToken cancellationToken)
    {
        var resources = _context.Tickets
            .WhereIf(!string.IsNullOrEmpty(request.TicketType), x => x.TicketType == request.TicketType)
            .WhereIf(!string.IsNullOrEmpty(request.Status), x => x.Status == request.Status)
            .WhereIf(request.SubscriptionId != null, x => x.SubscriptionId == request.SubscriptionId)
            .WhereIf(request.CreatedAt != default, x => x.CreatedAt == request.CreatedAt)
            .WhereIf(request.UpdatedAt != default, x => x.UpdatedAt == request.UpdatedAt)
            .Include(x => x.Subscription);

        return await resources.ProjectToType<GetTicketResponse>()
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}