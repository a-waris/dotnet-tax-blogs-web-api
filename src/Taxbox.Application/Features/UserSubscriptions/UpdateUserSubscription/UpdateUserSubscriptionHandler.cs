using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.UserSubscriptions.UpdateUserSubscription;

public class
    UpdateUserSubscriptionHandler : IRequestHandler<UpdateUserSubscriptionRequest, Result<GetUserSubscriptionResponse>>
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
        if (originalUserSubscription.SubscriptionId != request.SubscriptionId)
        {
            originalUserSubscription.SubscriptionId = request.SubscriptionId;
        }

        originalUserSubscription.SubscriptionStartDate = request.SubscriptionStartDate;
        originalUserSubscription.SubscriptionEndDate = request.SubscriptionEndDate;
        if (request.IsActive != null)
        {
            originalUserSubscription.IsActive = (bool)request.IsActive;
        }

        if (request.AutoRenewal != null)
        {
            originalUserSubscription.AutoRenewal = (bool)request.AutoRenewal;
        }

        // originalUserSubscription.UserId = request.UserId;
        // originalUserSubscription.CouponCode = request.CouponCode;
        // originalUserSubscription.DiscountAmount = request.DiscountAmount;
        // originalUserSubscription.NextBillingDate = request.NextBillingDate;
        originalUserSubscription.TrialEndDate = request.TrialEndDate;
        originalUserSubscription.CancellationDate = request.CancellationDate;
        originalUserSubscription.TrialStartDate = request.TrialStartDate;

        _context.UserSubscriptions.Update(originalUserSubscription);
        await _context.SaveChangesAsync(cancellationToken);
        return originalUserSubscription.Adapt<GetUserSubscriptionResponse>();
    }
}