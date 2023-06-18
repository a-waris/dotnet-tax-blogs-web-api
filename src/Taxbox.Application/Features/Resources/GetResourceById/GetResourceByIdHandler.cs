using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Resources.GetResourceById;

public class GetResourceByIdHandler : IRequestHandler<GetResourceByIdRequest, Result<GetResourceResponse>>
{
    private readonly IContext _context;


    public GetResourceByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetResourceResponse>> Handle(GetResourceByIdRequest request,
        CancellationToken cancellationToken)
    {
        var resource = await _context.Resources.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);
        if (resource is null) return Result.NotFound();
        return resource.Adapt<GetResourceResponse>();
    }
}