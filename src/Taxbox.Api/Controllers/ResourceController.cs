using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Resources;
using Taxbox.Application.Features.Resources.CreateResource;
using Taxbox.Application.Features.Resources.DeleteResource;
using Taxbox.Application.Features.Resources.GetAllResources;
using Taxbox.Application.Features.Resources.GetResourceById;
using Taxbox.Application.Features.Resources.UpdateResource;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ResourceController : ControllerBase
{
    private readonly IMediator _mediator;

    public ResourceController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetResourceResponse>> GetById(ResourceId id)
    {
        return Ok(await _mediator.Send(new GetResourceByIdRequest(id)));
    }


    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetResourceResponse>>> GetList(
        [FromQuery] GetAllResourcesRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetResourceResponse>> Create(
        [FromForm] CreateResourceRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetResourceResponse>> Update(
        ResourceId id,
        [FromBody] UpdateResourceRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(ResourceId id)
    {
        return Ok(await _mediator.Send(new DeleteResourceRequest(Id: id)));
    }
}