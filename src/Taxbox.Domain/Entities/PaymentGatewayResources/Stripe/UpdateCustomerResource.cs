using Stripe;

namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record UpdateCustomerResource(
    CustomerUpdateOptions CustomerUpdateOptions);