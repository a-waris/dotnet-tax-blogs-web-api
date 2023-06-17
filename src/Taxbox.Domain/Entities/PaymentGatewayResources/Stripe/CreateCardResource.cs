namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record CreateCardResource(
    string Name,
    string? Number,
    string ExpiryYear,
    string ExpiryMonth,
    string Cvc);