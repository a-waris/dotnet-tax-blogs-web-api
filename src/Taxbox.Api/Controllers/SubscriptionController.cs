using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Subscriptions;
using Taxbox.Application.Features.Subscriptions.CreateSubscription;
using Taxbox.Application.Features.Subscriptions.DeleteSubscription;
using Taxbox.Application.Features.Subscriptions.GetAllSubscriptions;
using Taxbox.Application.Features.Subscriptions.GetSubscriptionById;
using Taxbox.Application.Features.Subscriptions.UpdateSubscription;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public SubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetSubscriptionResponse>> GetById(SubscriptionId id)
    {
        return Ok(await _mediator.Send(new GetSubscriptionByIdRequest(id)));
    }


    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetSubscriptionResponse>>> GetList(
        [FromQuery] GetAllSubscriptionsRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetSubscriptionResponse>> Create(
        [FromForm] CreateSubscriptionRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetSubscriptionResponse>> Update(
        SubscriptionId id,
        [FromBody] UpdateSubscriptionRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(SubscriptionId id)
    {
        return Ok(await _mediator.Send(new DeleteSubscriptionRequest(Id: id)));
    }
}