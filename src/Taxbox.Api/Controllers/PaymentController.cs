using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;
using Taxbox.Domain.PaymentGateway.Interfaces;

namespace Taxbox.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PaymentController : ControllerBase
{
    private readonly IStripeService _stripeService;

    public PaymentController(IStripeService stripeService)
    {
        _stripeService = stripeService;
    }

    [HttpPost("customer")]
    public async Task<ActionResult<CustomerResource>> CreateCustomer([FromBody] CreateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        var response = await _stripeService.CreateCustomer(resource, cancellationToken);
        return Ok(response);
    }

    [HttpGet("customer")]
    [Route("{customerId}")]
    public async Task<ActionResult<Customer>> GetCustomer(string customerId,
        CancellationToken cancellationToken)
    {
        var response = await _stripeService.RetrieveCustomer(customerId, cancellationToken);
        return Ok(response);
    }

    [HttpPut("customer")]
    [Route("{customerId}")]
    public async Task<ActionResult<Customer>> UpdateCustomer(string customerId,
        [FromQuery] UpdateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        var response = await _stripeService.UpdateCustomer(customerId, resource, cancellationToken);
        return Ok(response);
    }

    [HttpDelete("customer")]
    [Route("{customerId}")]
    public async Task<ActionResult<Customer>> DeleteCustomer(string customerId,
        CancellationToken cancellationToken)
    {
        var response = await _stripeService.DeleteCustomer(customerId, cancellationToken);
        return Ok(response);
    }

    [HttpPost("charge")]
    public async Task<ActionResult<ChargeResource>> CreateCharge([FromBody] CreateChargeResource resource,
        CancellationToken cancellationToken)
    {
        var response = await _stripeService.CreateCharge(resource, cancellationToken);
        return Ok(response);
    }
}