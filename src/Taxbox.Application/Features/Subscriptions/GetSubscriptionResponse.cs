using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Subscriptions;

public record GetSubscriptionResponse
{
    public SubscriptionId Id { get; set; } 
    public string Name { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int ValidityPeriod { get; set; }
    public string ValidityPeriodType { get; set; } = null!;
    public string VAT { get; set; } = null!;
}