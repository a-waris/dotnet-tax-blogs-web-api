using Ardalis.Result;
using Mapster;
using MediatR;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Pages.CreatePage;

public class CreatePageHandler : IRequestHandler<CreatePageRequest, Result<GetPageResponse>>
{
    private readonly IContext _context;


    public CreatePageHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetPageResponse>> Handle(CreatePageRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Page>();
        _context.Pages.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetPageResponse>();
    }
}