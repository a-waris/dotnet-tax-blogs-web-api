using Ardalis.Result;
using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;

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
            if (originalPage.MetadataJson is "{}" or "{ }" or null)
            {
                var metadata = new Metadata()
                {
                    Category = request.Metadata.Category,
                    Language = request.Metadata.Language,
                    Views = request.Metadata.Views ?? 0
                };

                originalPage.MetadataJson = JsonSerializer.Serialize(metadata);
            }
            else
            {
                var metadata = JsonSerializer.Deserialize<Metadata>(originalPage.MetadataJson);
                if (metadata != null)
                {
                    metadata.Category = request.Metadata.Category;
                    metadata.Language = request.Metadata.Language;
                    metadata.Views = request.Metadata.Views ?? 0;
                    originalPage.MetadataJson = JsonSerializer.Serialize(metadata);
                }
            }
        }

        originalPage.UpdatedAt = DateTime.UtcNow;

        _context.Pages.Update(originalPage);
        await _context.SaveChangesAsync(cancellationToken);
        return originalPage.Adapt<GetPageResponse>();
    }
}