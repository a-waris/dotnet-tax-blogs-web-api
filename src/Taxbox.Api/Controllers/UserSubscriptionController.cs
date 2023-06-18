using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.UserSubscriptions;
using Taxbox.Application.Features.UserSubscriptions.CreateUserSubscription;
using Taxbox.Application.Features.UserSubscriptions.DeleteUserSubscription;
using Taxbox.Application.Features.UserSubscriptions.GetAllUserSubscriptions;
using Taxbox.Application.Features.UserSubscriptions.GetUserSubscriptionById;
using Taxbox.Application.Features.UserSubscriptions.UpdateUserSubscription;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserSubscriptionController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserSubscriptionController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetUserSubscriptionResponse>> GetById(UserSubscriptionId id)
    {
        return Ok(await _mediator.Send(new GetUserSubscriptionByIdRequest(id)));
    }


    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetUserSubscriptionResponse>>> GetList(
        [FromQuery] GetAllUserSubscriptionsRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetUserSubscriptionResponse>> Create(
        [FromForm] CreateUserSubscriptionRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetUserSubscriptionResponse>> Update(
        UserSubscriptionId id,
        [FromBody] UpdateUserSubscriptionRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(UserSubscriptionId id)
    {
        return Ok(await _mediator.Send(new DeleteUserSubscriptionRequest(Id: id)));
    }
}