﻿using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Taxbox.Application.Common.Responses;
using Taxbox.Application.Features.Heroes;
using System.Threading.Tasks;
using Taxbox.Application.Features.Heroes.CreateHero;
using Taxbox.Application.Features.Heroes.DeleteHero;
using Taxbox.Application.Features.Heroes.GetAllHeroes;
using Taxbox.Application.Features.Heroes.GetHeroById;
using Taxbox.Application.Features.Heroes.UpdateHero;
using Taxbox.Domain.Entities.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Taxbox.Api.Controllers;

[ApiController]
[AllowAnonymous]
[Route("api/[controller]")]
public class HeroController : ControllerBase
{
    private readonly IMediator _mediator;

    public HeroController(IMediator mediator)
    {
        _mediator = mediator;
    }


    /// <summary>
    /// Returns all heroes in the database
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet]
    [TranslateResultToActionResult]
    public async Task<ActionResult<PaginatedList<GetHeroResponse>>> GetHeroes([FromQuery] GetAllHeroesRequest request)
    {
        return Ok(await _mediator.Send(request));
    }


    /// <summary>
    /// Get one hero by id from the database
    /// </summary>
    /// <param name="id">The hero's ID</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.NotFound)]
    public async Task<Result<GetHeroResponse>> GetHeroById(HeroId id)
    {
        var result = await _mediator.Send(new GetHeroByIdRequest(id));
        return result;
    }

    /// <summary>
    /// Insert one hero in the database
    /// </summary>
    /// <param name="request">The hero information</param>
    /// <returns></returns>
    /// 
    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<Result<GetHeroResponse>> Create([FromBody] CreateHeroRequest request)
    {
        var result = await _mediator.Send(request);
        return result;

    }

    /// <summary>
    /// Update a hero from the database
    /// </summary>
    /// <param name="id">The hero's ID</param>
    /// <param name="request">The update object</param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound)]
    public async Task<Result<GetHeroResponse>> Update(HeroId id, [FromBody] UpdateHeroRequest request)
    {
        var result = await _mediator.Send(request with { Id = id });
        return result;
    }


    /// <summary>
    /// Delete a hero from the database
    /// </summary>
    /// <param name="id">The hero's ID</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid, ResultStatus.NotFound)]
    public async Task<Result> Delete(HeroId id)
    {
        var result = await _mediator.Send(new DeleteHeroRequest(id));
        return result;
    }
}