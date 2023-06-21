using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Pages;
using Taxbox.Application.Features.Pages.CreatePage;
using Taxbox.Application.Features.Pages.DeletePage;
using Taxbox.Application.Features.Pages.GetAllPages;
using Taxbox.Application.Features.Pages.GetPageById;
using Taxbox.Application.Features.Pages.UpdatePage;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PageController : ControllerBase
{
    private readonly IMediator _mediator;

    public PageController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetPageResponse>> GetById(PageId id)
    {
        return Ok(await _mediator.Send(new GetPageByIdRequest(id)));
    }


    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetPageResponse>>> GetList(
        [FromQuery] GetAllPagesRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetPageResponse>> Create(
        [FromForm] CreatePageRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetPageResponse>> Update(
        PageId id,
        [FromBody] UpdatePageRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(PageId id)
    {
        return Ok(await _mediator.Send(new DeletePageRequest(Id: id)));
    }
}