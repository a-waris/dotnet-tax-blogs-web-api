using Ardalis.Result;
using MediatR;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Pages.DeletePage;

public record DeletePageRequest(PageId Id) : IRequest<Result>;