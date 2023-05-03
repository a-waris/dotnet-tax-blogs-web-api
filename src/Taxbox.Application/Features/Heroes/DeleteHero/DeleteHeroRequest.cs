using Ardalis.Result;
using Taxbox.Domain.Entities.Common;
using MediatR;

namespace Taxbox.Application.Features.Heroes.DeleteHero;

public record DeleteHeroRequest(HeroId Id) : IRequest<Result>;