using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Pages.GetPageById;

public record GetPageByIdRequest(PageId Id) : IRequest<Result<GetPageResponse>>;