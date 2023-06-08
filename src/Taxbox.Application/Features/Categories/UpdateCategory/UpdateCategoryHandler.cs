using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Categories.UpdateCategory;

public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryRequest, Result<GetCategoryResponse>>
{
    private readonly IContext _context;

    public UpdateCategoryHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetCategoryResponse>> Handle(UpdateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var originalCategory = await _context.Categories
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (originalCategory == null) return Result.NotFound();

        originalCategory.Name = request.Name;
        originalCategory.Description = request.Description;

        _context.Categories.Update(originalCategory);
        await _context.SaveChangesAsync(cancellationToken);
        return originalCategory.Adapt<GetCategoryResponse>();
    }
}