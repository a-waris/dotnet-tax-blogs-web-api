using Ardalis.Result;
using Mapster;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;

namespace Taxbox.Application.Features.Categories.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryRequest, Result<GetCategoryResponse>>
{
    private readonly IContext _context;


    public CreateCategoryHandler(IContext context)
    {
        _context = context;
    }

    public async Task<Result<GetCategoryResponse>> Handle(CreateCategoryRequest request,
        CancellationToken cancellationToken)
    {
        var created = request.Adapt<Domain.Entities.Category>();
        _context.Categories.Add(created);
        await _context.SaveChangesAsync(cancellationToken);
        return created.Adapt<GetCategoryResponse>();
    }
}