namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record CustomerResource(
    string CustomerId,
    string Email,
    string Name);