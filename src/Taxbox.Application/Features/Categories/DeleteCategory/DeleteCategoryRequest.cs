using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Categories.DeleteCategory;

public record DeleteCategoryRequest(CategoryId Id) : IRequest<Result>;