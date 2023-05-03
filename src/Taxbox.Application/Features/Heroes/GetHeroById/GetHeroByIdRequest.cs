using Ardalis.Result;
using Taxbox.Domain.Entities.Common;
using MediatR;

namespace Taxbox.Application.Features.Heroes.GetHeroById;

public record GetHeroByIdRequest(HeroId Id) : IRequest<Result<GetHeroResponse>>;