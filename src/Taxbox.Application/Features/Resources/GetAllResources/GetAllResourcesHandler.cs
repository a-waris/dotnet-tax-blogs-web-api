using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Extensions;

namespace Taxbox.Application.Features.Resources.GetAllResources;

public class GetAllResourcesHandler : IRequestHandler<GetAllResourcesRequest, PaginatedList<GetResourceResponse>>
{
    private readonly IContext _context;

    public GetAllResourcesHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetResourceResponse>> Handle(GetAllResourcesRequest request,
        CancellationToken cancellationToken)
    {
        var resources = _context.Resources
            .WhereIf(!string.IsNullOrEmpty(request.DisplayName),
                x => EF.Functions.Like(x.DisplayName, $"%{request.DisplayName}%"))
            .WhereIf(request.CategoryId != null, x => x.CategoryId == request.CategoryId)
            .WhereIf(request.ResourceType != null, x => x.ResourceType == request.ResourceType)
            .Include(x => x.Category);

        return await resources.ProjectToType<GetResourceResponse>()
            .OrderBy(x => x.DisplayName)
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}