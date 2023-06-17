using Microsoft.Extensions.Configuration;
using Stripe;
using System;
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
    private readonly PaymentIntentService _paymentIntentService;
    private readonly IConfiguration _configuration;

    public StripeService(
        TokenService tokenService,
        CustomerService customerService,
        ChargeService chargeService,
        PaymentIntentService paymentIntentService,
        IConfiguration configuration)

    {
        _tokenService = tokenService;
        _customerService = customerService;
        _chargeService = chargeService;
        _paymentIntentService = paymentIntentService;
        _configuration = configuration;
    }

    public async Task<CustomerResource> CreateCustomer(CreateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        try
        {
            var mode = _configuration.GetSection("StripeOptions")["Mode"];
            var customerOptions = new CustomerCreateOptions { Email = resource.Email, Name = resource.Name };
            if (mode == "live")
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
                customerOptions.Source = token.Id;
            }
            else
            {
                customerOptions.PaymentMethod = "pm_card_visa";
            }

            var customer =
                await _customerService.CreateAsync(customerOptions, cancellationToken: cancellationToken);

            return new CustomerResource(customer.Id, customer.Email, customer.Name);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<CustomerResource> RetrieveCustomer(string customerId,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.GetAsync(customerId, cancellationToken: cancellationToken);

        return new CustomerResource(customer.Id, customer.Email, customer.Name);
    }

    public async Task<CustomerResource> UpdateCustomer(string customerId, UpdateCustomerResource resource,
        CancellationToken cancellationToken)
    {
        var customer = await _customerService.UpdateAsync(customerId, resource.CustomerUpdateOptions,
            cancellationToken: cancellationToken);

        return new CustomerResource(customer.Id, customer.Email, customer.Name);
    }

    public async Task<bool> DeleteCustomer(string customerId, CancellationToken cancellationToken)
    {
        var customer = await _customerService.DeleteAsync(customerId, cancellationToken: cancellationToken);
        return customer.Deleted ?? false;
    }

    public async Task<ChargeResource> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken)
    {
        var chargeOptions = new ChargeCreateOptions
        {
            Currency = resource.Currency,
            Amount = resource.Amount,
            ReceiptEmail = resource.ReceiptEmail,
            Customer = resource.CustomerId,
            Description = resource.Description
        };


        var charge = await _chargeService.CreateAsync(chargeOptions, null, cancellationToken);

        return new ChargeResource(
            charge.Id,
            charge.Currency,
            charge.Amount,
            charge.CustomerId,
            charge.ReceiptEmail,
            charge.Description);
    }

    public async Task<IntentResource> CreatePaymentIntent(CreatePaymentIntentResource resource,
        CancellationToken cancellationToken)
    {
        var options = new PaymentIntentCreateOptions
        {
            Currency = resource.Currency,
            Amount = resource.Amount,
            ReceiptEmail = resource.ReceiptEmail,
            Description = resource.Description,
            PaymentMethod = "pm_card_visa"
        };


        var intent = await _paymentIntentService.CreateAsync(options, null, cancellationToken);

        return new IntentResource(
            intent.Id,
            intent.Currency,
            intent.Amount,
            intent.CustomerId,
            intent.ReceiptEmail,
            intent.Description);
    }
}