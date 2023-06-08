using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Categories.GetCategoryById;

public record GetCategoryByIdRequest(CategoryId Id) : IRequest<Result<GetCategoryResponse>>;