using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Pages.DeletePage;

public class DeletePageHandler : IRequestHandler<DeletePageRequest, Result>
{
    private readonly IContext _context;

    public DeletePageHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeletePageRequest request, CancellationToken cancellationToken)
    {
        var page = await _context.Pages.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (page is null) return Result.NotFound();
        _context.Pages.Remove(page);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}