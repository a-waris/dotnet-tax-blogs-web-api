using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Extensions;

namespace Taxbox.Application.Features.Subscriptions.GetAllSubscriptions;

public class
    GetAllSubscriptionsHandler : IRequestHandler<GetAllSubscriptionsRequest, PaginatedList<GetSubscriptionResponse>>
{
    private readonly IContext _context;

    public GetAllSubscriptionsHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetSubscriptionResponse>> Handle(GetAllSubscriptionsRequest request,
        CancellationToken cancellationToken)
    {
        var subs = _context.Subscriptions
            .WhereIf(!string.IsNullOrEmpty(request.Name),
                x => EF.Functions.Like(x.Name, $"%{request.Name}%"))
            .WhereIf(!string.IsNullOrEmpty(request.Description),
                x => x.Description != null && EF.Functions.Like(x.Description, $"%{request.Description}%"))
            .WhereIf(request.Status != null, x => x.Status == request.Status)
            .WhereIf(request.Currency != null, x => x.Currency == request.Currency)
            .WhereIf(request.Amount != null, x => x.Amount == request.Amount)
            .WhereIf(request.ValidityPeriod != null, x => x.ValidityPeriod == request.ValidityPeriod)
            .WhereIf(request.ValidityPeriodType != null, x => x.ValidityPeriodType == request.ValidityPeriodType);


        return await subs.ProjectToType<GetSubscriptionResponse>()
            .OrderBy(x => x.Name)
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}