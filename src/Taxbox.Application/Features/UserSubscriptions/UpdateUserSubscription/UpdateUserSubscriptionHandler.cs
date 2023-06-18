using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.UserSubscriptions.UpdateUserSubscription;

public class UpdateUserSubscriptionHandler : IRequestHandler<UpdateUserSubscriptionRequest, Result<GetUserSubscriptionResponse>>
{
    private readonly IContext _context;

    public UpdateUserSubscriptionHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetUserSubscriptionResponse>> Handle(UpdateUserSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var originalUserSubscription = await _context.UserSubscriptions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (originalUserSubscription == null) return Result.NotFound();

        // TODO: add logic to update stripe subscription
        // TODO: do not allow to update subscription id or user id
        originalUserSubscription.SubscriptionId = request.SubscriptionId;
        originalUserSubscription.UserId = request.UserId;
        originalUserSubscription.SubscriptionStartDate = request.SubscriptionStartDate;
        originalUserSubscription.SubscriptionEndDate = request.SubscriptionEndDate;
        originalUserSubscription.IsActive = request.IsActive;
        originalUserSubscription.AutoRenewal = request.AutoRenewal;
        originalUserSubscription.CouponCode = request.CouponCode;
        originalUserSubscription.DiscountAmount = request.DiscountAmount;
        originalUserSubscription.TrialStartDate = request.TrialStartDate;
        originalUserSubscription.TrialEndDate = request.TrialEndDate;
        originalUserSubscription.CancellationDate = request.CancellationDate;
        originalUserSubscription.NextBillingDate = request.NextBillingDate;

        _context.UserSubscriptions.Update(originalUserSubscription);
        await _context.SaveChangesAsync(cancellationToken);
        return originalUserSubscription.Adapt<GetUserSubscriptionResponse>();
    }
}