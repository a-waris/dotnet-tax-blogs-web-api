using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Taxbox.Application.Features.Tickets;
using Taxbox.Application.Features.Tickets.CreateTicket;
using Taxbox.Application.Features.Tickets.DeleteTicket;
using Taxbox.Application.Features.Tickets.GetAllTickets;
using Taxbox.Application.Features.Tickets.GetTicketById;
using Taxbox.Application.Features.Tickets.UpdateTicket;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TicketController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetTicketResponse>> GetById(TicketId id)
    {
        return Ok(await _mediator.Send(new GetTicketByIdRequest(id)));
    }


    [HttpGet]
    [Route("list")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<IEnumerable<GetTicketResponse>>> GetList(
        [FromQuery] GetAllTicketsRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetTicketResponse>> Create(
        [FromBody] CreateTicketRequest request)
    {
        return Ok(await _mediator.Send(request));
    }

    [HttpPut]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult<GetTicketResponse>> Update(
        TicketId id,
        [FromBody] UpdateTicketRequest request)
    {
        return Ok(await _mediator.Send(request with { Id = id }));
    }

    [HttpDelete]
    [Route("{id}")]
    [TranslateResultToActionResult]
    [ExpectedFailures(ResultStatus.Invalid)]
    public async Task<ActionResult> Remove(TicketId id)
    {
        return Ok(await _mediator.Send(new DeleteTicketRequest(Id: id)));
    }
}