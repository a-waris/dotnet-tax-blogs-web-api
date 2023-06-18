namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record CreateCustomerResource(
    string Email,
    string Name,
    CreateCardResource Card);