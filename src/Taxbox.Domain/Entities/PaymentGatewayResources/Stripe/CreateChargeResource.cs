using Stripe;

namespace Taxbox.Domain.Entities.PaymentGatewayResources.Stripe;

public record CreateChargeResource(
    ChargeCreateOptions ChargeCreateOptions
    // string Currency,
    // long Amount,
    // string CustomerId,
    // string ReceiptEmail,
    // string Description
);