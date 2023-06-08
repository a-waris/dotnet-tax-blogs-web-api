using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Extensions;

namespace Taxbox.Application.Features.Categories.GetAllCategories;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesRequest, PaginatedList<GetCategoryResponse>>
{
    private readonly IContext _context;

    public GetAllCategoriesHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetCategoryResponse>> Handle(GetAllCategoriesRequest request,
        CancellationToken cancellationToken)
    {
        var resources = _context.Categories
            .WhereIf(!string.IsNullOrEmpty(request.Name),
                x => EF.Functions.Like(x.Name, $"%{request.Name}%"))
            .WhereIf(!string.IsNullOrEmpty(request.Description),
                x => EF.Functions.Like(x.Description, $"%{request.Description}%"))
            .WhereIf(request.Status != null, x => x.Status == request.Status);

        return await resources.ProjectToType<GetCategoryResponse>()
            .OrderBy(x => x.Name)
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}