namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record CreateChargeResource(
    string Currency,
    long Amount,
    string CustomerId,
    string ReceiptEmail,
    string Description);