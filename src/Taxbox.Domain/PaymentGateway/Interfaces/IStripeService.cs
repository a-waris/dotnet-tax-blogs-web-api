using Stripe;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

namespace Taxbox.Domain.PaymentGateway.Interfaces;

public interface IStripeService
{
    Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken);
    Task<CustomerResource> RetrieveCustomer(string customerId, CancellationToken cancellationToken);

    Task<CustomerResource> UpdateCustomer(string customerId, UpdateCustomerResource resource,
        CancellationToken cancellationToken);

    Task<bool> DeleteCustomer(string customerId, CancellationToken cancellationToken);
    Task<ChargeResource> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken);

    Task<IntentResource> CreatePaymentIntent(CreatePaymentIntentResource resource,
        CancellationToken cancellationToken);
}