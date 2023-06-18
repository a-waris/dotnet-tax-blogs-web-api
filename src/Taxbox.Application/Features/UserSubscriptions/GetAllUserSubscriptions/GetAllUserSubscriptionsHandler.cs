using Mapster;
using MediatR;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Extensions;

namespace Taxbox.Application.Features.UserSubscriptions.GetAllUserSubscriptions;

public class
    GetAllSubscriptionsHandler : IRequestHandler<GetAllUserSubscriptionsRequest,
        PaginatedList<GetUserSubscriptionResponse>>
{
    private readonly IContext _context;

    public GetAllSubscriptionsHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetUserSubscriptionResponse>> Handle(GetAllUserSubscriptionsRequest request,
        CancellationToken cancellationToken)
    {
        var ctx = _context.UserSubscriptions;
        var userSubs = _context.UserSubscriptions
            .WhereIf(request.SubscriptionStartDate != null && request.SubscriptionEndDate == null,
                x => x.SubscriptionStartDate >= request.SubscriptionStartDate &&
                     x.SubscriptionEndDate <= request.SubscriptionEndDate)
            .WhereIf(request.SubscriptionStartDate != null,
                x => x.SubscriptionStartDate == request.SubscriptionStartDate)
            .WhereIf(request.SubscriptionEndDate != null,
                x => x.SubscriptionEndDate == request.SubscriptionEndDate)
            .WhereIf(request.IsActive != null, x => x.IsActive == request.IsActive)
            .WhereIf(request.UserId != null, x => x.UserId == request.UserId)
            .WhereIf(request.SubscriptionId != null, x => x.SubscriptionId == request.SubscriptionId)
            .WhereIf(request.AutoRenewal != null, x => x.AutoRenewal == request.AutoRenewal)
            .WhereIf(request.CancellationDate != null, x => x.CancellationDate == request.CancellationDate)
            .WhereIf(request.TrialEndDate != null && request.TrialStartDate == null,
                x => x.TrialEndDate >= request.TrialEndDate && x.TrialStartDate <= request.TrialEndDate)
            .WhereIf(request.TrialStartDate != null,
                x => x.TrialStartDate == request.TrialStartDate)
            .WhereIf(request.TrialEndDate != null,
                x => x.TrialEndDate == request.TrialEndDate)
            .WhereIf(request.CouponCode != null, x => x.CouponCode == request.CouponCode)
            .WhereIf(request.NextBillingDate != null, x => x.NextBillingDate == request.NextBillingDate)
            .WhereIf(request.DiscountAmount != null, x => x.DiscountAmount == request.DiscountAmount);


        return await userSubs.ProjectToType<GetUserSubscriptionResponse>()
            .OrderBy(x => x.SubscriptionEndDate)
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}