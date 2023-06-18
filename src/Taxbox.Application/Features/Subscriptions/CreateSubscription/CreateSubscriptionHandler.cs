using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Subscriptions.CreateSubscription;

public class CreateSubscriptionHandler : IRequestHandler<CreateSubscriptionRequest, Result<GetSubscriptionResponse>>
{
    private readonly IContext _context;


    public CreateSubscriptionHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetSubscriptionResponse>> Handle(CreateSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Subscription>();

        _context.Subscriptions.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetSubscriptionResponse>();
    }
}