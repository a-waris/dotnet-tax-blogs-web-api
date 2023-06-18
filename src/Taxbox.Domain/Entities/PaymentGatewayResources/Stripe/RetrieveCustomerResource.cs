using Stripe;

namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record RetrieveCustomerResource(
    CustomerGetOptions? CustomerGetOptions
);