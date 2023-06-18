using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.UserSubscriptions.GetUserSubscriptionById;

public class GetUserSubscriptionByIdHandler : IRequestHandler<GetUserSubscriptionByIdRequest, Result<GetUserSubscriptionResponse>>
{
    private readonly IContext _context;


    public GetUserSubscriptionByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetUserSubscriptionResponse>> Handle(GetUserSubscriptionByIdRequest request,
        CancellationToken cancellationToken)
    {
        var sub = await _context.UserSubscriptions.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);
        if (sub is null) return Result.NotFound();
        return sub.Adapt<GetUserSubscriptionResponse>();
    }
}