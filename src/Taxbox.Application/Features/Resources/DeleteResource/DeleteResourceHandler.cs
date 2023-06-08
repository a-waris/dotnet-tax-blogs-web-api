using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Resources.DeleteResource;

public class DeleteResourceHandler : IRequestHandler<DeleteResourceRequest, Result>
{
    private readonly IContext _context;

    public DeleteResourceHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteResourceRequest request, CancellationToken cancellationToken)
    {
        var resource = await _context.Resources.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (resource is null) return Result.NotFound();
        _context.Resources.Remove(resource);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}