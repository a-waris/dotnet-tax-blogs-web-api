using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Extensions;

namespace Taxbox.Application.Features.Pages.GetAllPages;

public class GetAllPagesHandler : IRequestHandler<GetAllPagesRequest, PaginatedList<GetPageResponse>>
{
    private readonly IContext _context;

    public GetAllPagesHandler(IContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetPageResponse>> Handle(GetAllPagesRequest request,
        CancellationToken cancellationToken)
    {
        var resources = _context.Pages
            .WhereIf(!string.IsNullOrEmpty(request.Label),
                x => EF.Functions.Like(x.Label, $"%{request.Label}%"))
            .WhereIf(string.IsNullOrEmpty(request.Slug),
                x => EF.Functions.Like(x.Slug, $"%{request.Slug}%"))
            .WhereIf(string.IsNullOrEmpty(request.ParentName),
                x => EF.Functions.Like(x.ParentName, $"%{request.ParentName}%"));


        return await resources.ProjectToType<GetPageResponse>()
            .ToPaginatedListAsync(request.CurrentPage, request.PageSize);
    }
}