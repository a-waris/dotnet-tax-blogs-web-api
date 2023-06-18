using Stripe;

namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record CreatePaymentIntentResource(
    string Currency,
    long Amount,
    string ReceiptEmail,
    string Description
);