using Stripe;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;
using Taxbox.Domain.PaymentGateway.Interfaces;

namespace Taxbox.Infrastructure.PaymentGateway.Services;

public class StripeService : IStripeService
{
    private readonly TokenService _tokenService;
    private readonly CustomerService _customerService;
    private readonly ChargeService _chargeService;

    public StripeService(
        TokenService tokenService,
        CustomerService customerService,
        ChargeService chargeService)
    {
        _tokenService = tokenService;
        _customerService = customerService;
        _chargeService = chargeService;
    }

    public async Task<CustomerResource> CreateCustomer(CreateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        var tokenOptions = new TokenCreateOptions
        {
            Card = new TokenCardOptions
            {
                Name = resource.Card.Name,
                Number = resource.Card.Number,
                ExpYear = resource.Card.ExpiryYear,
                ExpMonth = resource.Card.ExpiryMonth,
                Cvc = resource.Card.Cvc
            }
        };
        var token = await _tokenService.CreateAsync(tokenOptions, cancellationToken: cancellationToken);

        var customerOptions = new CustomerCreateOptions
        {
            Email = resource.Email, Name = resource.Name, Source = token.Id
        };
        var customer = await _customerService.CreateAsync(customerOptions, cancellationToken: cancellationToken);

        return new CustomerResource(customer.Id, customer.Email, customer.Name);
    }

    public async Task<Customer> RetrieveCustomer(string customerId,
        CancellationToken cancellationToken)
    {
        return await _customerService.GetAsync(customerId, cancellationToken: cancellationToken);
    }

    public async Task<Customer> UpdateCustomer(string customerId, UpdateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        Customer customer = await _customerService.UpdateAsync(customerId, resource.CustomerUpdateOptions,
            cancellationToken: cancellationToken);

        return customer;
    }

    public async Task<Customer> DeleteCustomer(string customerId, CancellationToken cancellationToken)
    {
        return await _customerService.DeleteAsync(customerId, cancellationToken: cancellationToken);
    }

    public async Task<ChargeResource> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken)
    {
        // var chargeOptions = new ChargeCreateOptions
        // {
        //     Currency = resource.Currency,
        //     Amount = resource.Amount,
        //     ReceiptEmail = resource.ReceiptEmail,
        //     Customer = resource.CustomerId,
        //     Description = resource.Description
        // };

        var charge = await _chargeService.CreateAsync(resource.ChargeCreateOptions, null, cancellationToken);

        return new ChargeResource(
            charge.Id,
            charge.Currency,
            charge.Amount,
            charge.CustomerId,
            charge.ReceiptEmail,
            charge.Description);
    }
}