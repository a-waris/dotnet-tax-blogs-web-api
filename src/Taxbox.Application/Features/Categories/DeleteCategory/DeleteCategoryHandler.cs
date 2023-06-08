using Ardalis.Result;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Categories.DeleteCategory;

public class DeleteCatgoryHandler : IRequestHandler<DeleteCategoryRequest, Result>
{
    private readonly IContext _context;

    public DeleteCatgoryHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result> Handle(DeleteCategoryRequest request, CancellationToken cancellationToken)
    {
        var resource = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (resource is null) return Result.NotFound();
        _context.Categories.Remove(resource);
        await _context.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}