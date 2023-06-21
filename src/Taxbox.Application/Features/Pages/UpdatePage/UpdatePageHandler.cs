using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Pages.UpdatePage;

public class UpdatePageHandler : IRequestHandler<UpdatePageRequest, Result<GetPageResponse>>
{
    private readonly IContext _context;

    public UpdatePageHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetPageResponse>> Handle(UpdatePageRequest request,
        CancellationToken cancellationToken)
    {
        var originalPage = await _context.Pages
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (originalPage == null) return Result.NotFound();

        originalPage.Label = request.Label;
        originalPage.Slug = request.Slug;
        originalPage.HtmlContent = request.HtmlContent;
        originalPage.ParentName = request.ParentName;
        originalPage.Metadata = request.Metadata;
        originalPage.UpdatedAt = DateTime.UtcNow;

        _context.Pages.Update(originalPage);
        await _context.SaveChangesAsync(cancellationToken);
        return originalPage.Adapt<GetPageResponse>();
    }
}