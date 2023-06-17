using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Features.Categories;

namespace Taxbox.Application.Features.Subscriptions.GetSubscriptionById;

public class GetSubscriptionByIdHandler : IRequestHandler<GetSubscriptionByIdRequest, Result<GetSubscriptionResponse>>
{
    private readonly IContext _context;


    public GetSubscriptionByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetSubscriptionResponse>> Handle(GetSubscriptionByIdRequest request,
        CancellationToken cancellationToken)
    {
        var sub = await _context.Subscriptions.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);
        if (sub is null) return Result.NotFound();
        return sub.Adapt<GetSubscriptionResponse>();
    }
}