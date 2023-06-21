using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Pages.GetPageById;

public class GetPageByIdHandler : IRequestHandler<GetPageByIdRequest, Result<GetPageResponse>>
{
    private readonly IContext _context;


    public GetPageByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetPageResponse>> Handle(GetPageByIdRequest request,
        CancellationToken cancellationToken)
    {
        var resource = await _context.Pages.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);
        if (resource is null) return Result.NotFound();
        return resource.Adapt<GetPageResponse>();
    }
}