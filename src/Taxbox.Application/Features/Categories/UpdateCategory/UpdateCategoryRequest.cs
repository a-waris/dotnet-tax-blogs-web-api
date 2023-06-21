using Ardalis.Result;
using MediatR;
using System.Text.Json.Serialization;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Categories.UpdateCategory;

public record UpdateCategoryRequest : IRequest<Result<GetCategoryResponse>>
{
    [JsonIgnore] public CategoryId Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? Status { get; set; } = true;
}