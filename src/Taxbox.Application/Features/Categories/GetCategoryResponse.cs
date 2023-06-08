using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Categories;

public record GetCategoryResponse
{
    public CategoryId Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool Status { get; set; }
}