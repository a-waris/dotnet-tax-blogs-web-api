using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.UserSubscriptions.DeleteUserSubscription;

public class DeleteUserSubscriptionHandler : IRequestHandler<DeleteUserSubscriptionRequest, Result>
{
    private readonly IContext _context;

    public DeleteUserSubscriptionHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteUserSubscriptionRequest request, CancellationToken cancellationToken)
    {
        var useSub = await _context.UserSubscriptions.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (useSub is null) return Result.NotFound();
        _context.UserSubscriptions.Remove(useSub);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}