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

        if (!string.IsNullOrEmpty(request.Label))
        {
            originalPage.Label = request.Label;
        }

        if (!string.IsNullOrEmpty(request.Slug))
        {
            originalPage.Slug = request.Slug;
        }

        if (!string.IsNullOrEmpty(request.HtmlContent))
        {
            originalPage.HtmlContent = request.HtmlContent;
        }

        if (!string.IsNullOrEmpty(request.ParentName))
        {
            originalPage.ParentName = request.ParentName;
        }

        if (request.Metadata != null)
        {
            originalPage.Metadata = request.Metadata;
        }

        originalPage.Metadata = request.Metadata;
        originalPage.UpdatedAt = DateTime.UtcNow;

        _context.Pages.Update(originalPage);
        await _context.SaveChangesAsync(cancellationToken);
        return originalPage.Adapt<GetPageResponse>();
    }
}