using System.Threading;
using System.Threading.Tasks;
using Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

namespace Taxbox.Domain.PaymentGateway.Interfaces;

public interface IStripeService
{
    Task<CustomerResource> CreateCustomer(CreateCustomerResource resource, CancellationToken cancellationToken);
    Task<ChargeResource> CreateCharge(CreateChargeResource resource, CancellationToken cancellationToken);
}