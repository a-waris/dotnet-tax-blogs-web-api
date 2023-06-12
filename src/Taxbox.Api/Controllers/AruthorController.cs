using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Authors;
using Taxbox.Application.Features.Authors.CreateAuthor;
using Taxbox.Application.Features.Authors.DeleteAuthor;
using Taxbox.Application.Features.Authors.GetAllAuthor;
using Taxbox.Application.Features.Authors.GetAuthorById;
using Taxbox.Application.Features.Authors.UpdateAuthor;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthorController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetAuthorResponse>> GetById(AuthorId id)
    {
        return Ok(await _mediator.Send(new GetAuthorByIdRequest(id)));
    }

    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetAuthorResponse>>> GetList(
        [FromQuery] GetAllAuthorsRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetAuthorResponse>> Create(
        [FromForm] CreateAuthorRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetAuthorResponse>> Update(
        AuthorId id,
        [FromBody] UpdateAuthorRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(AuthorId id)
    {
        return Ok(await _mediator.Send(new DeleteAuthorRequest(Id: id)));
    }
}