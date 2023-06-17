using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;

public class
    CreateUserSubscriptionHandler : IRequestHandler<CreateUserSubscriptionRequest, Result<GetUserSubscriptionResponse>>
{
    private readonly IContext _context;


    public CreateUserSubscriptionHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetUserSubscriptionResponse>> Handle(CreateUserSubscriptionRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.UserSubscription>();

        _context.UserSubscriptions.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetUserSubscriptionResponse>();
    }
}