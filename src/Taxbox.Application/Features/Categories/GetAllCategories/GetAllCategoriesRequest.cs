using MediatR;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;

namespace Taxbox.Application.Features.Categories.GetAllCategories;

public record GetAllCategoriesRequest : PaginatedRequest, IRequest<PaginatedList<GetCategoryResponse>>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? Status { get; set; } = true;
}