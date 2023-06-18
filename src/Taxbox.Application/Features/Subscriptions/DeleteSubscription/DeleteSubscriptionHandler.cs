using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Subscriptions.DeleteSubscription;

public class DeleteSubscriptionHandler : IRequestHandler<DeleteSubscriptionRequest, Result>
{
    private readonly IContext _context;

    public DeleteSubscriptionHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var sub = await _context.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (sub is null) return Result.NotFound();
        _context.Subscriptions.Remove(sub);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}