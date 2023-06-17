using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Subscriptions.UpdateSubscription;

public class UpdateSubscriptionHandler : IRequestHandler<UpdateSubscriptionRequest, Result<GetSubscriptionResponse>>
{
    private readonly IContext _context;

    public UpdateSubscriptionHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetSubscriptionResponse>> Handle(UpdateSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var originalSubscription = await _context.Subscriptions
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (originalSubscription == null) return Result.NotFound();

        originalSubscription.Name = request.Name;
        originalSubscription.Description = request.Description;

        _context.Subscriptions.Update(originalSubscription);
        await _context.SaveChangesAsync(cancellationToken);
        return originalSubscription.Adapt<GetSubscriptionResponse>();
    }
}