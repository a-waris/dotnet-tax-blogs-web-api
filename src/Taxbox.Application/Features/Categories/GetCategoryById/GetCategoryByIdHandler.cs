using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Features.Resources;

namespace Taxbox.Application.Features.Categories.GetCategoryById;

public class GetCategoryByIdHandler : IRequestHandler<GetCategoryByIdRequest, Result<GetCategoryResponse>>
{
    private readonly IContext _context;


    public GetCategoryByIdHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetCategoryResponse>> Handle(GetCategoryByIdRequest request,
        CancellationToken cancellationToken)
    {
        var category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == request.Id,
            cancellationToken: cancellationToken);
        if (category is null) return Result.NotFound();
        return category.Adapt<GetCategoryResponse>();
    }
}