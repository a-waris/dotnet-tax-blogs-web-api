using Ardalis.Result;
using MediatR;

namespace Taxbox.Application.Features.Categories.CreateCategory;

public record CreateCategoryRequest : IRequest<Result<GetCategoryResponse>>
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Status { get; set; }
}