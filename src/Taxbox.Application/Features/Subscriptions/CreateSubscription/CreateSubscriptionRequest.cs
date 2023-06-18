using Ardalis.Result;
using MediatR;

namespace Taxbox.Application.Features.Subscriptions.CreateSubscription;

public record CreateSubscriptionRequest : IRequest<Result<GetSubscriptionResponse>>
{
    public string Name { get; set; } = null!;
    public string Status { get; set; } = null!;
    public string Currency { get; set; } = null!;
    public decimal Amount { get; set; }
    public string? Description { get; set; }
    public int ValidityPeriod { get; set; }
    public string ValidityPeriodType { get; set; } = null!;
}