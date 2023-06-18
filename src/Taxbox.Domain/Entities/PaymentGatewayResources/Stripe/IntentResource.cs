namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record IntentResource( string ChargeId,
    string Currency,
    long Amount,
    string CustomerId,
    string ReceiptEmail,
    string Description);