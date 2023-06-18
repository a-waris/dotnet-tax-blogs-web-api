using MediatR;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;

namespace Taxbox.Application.Features.Subscriptions.GetAllSubscriptions;

public record GetAllSubscriptionsRequest : PaginatedRequest, IRequest<PaginatedList<GetSubscriptionResponse>>
{
    public string? Name { get; set; }
    public string? Status { get; set; }
    public string? Currency { get; set; }
    public decimal? Amount { get; set; }
    public string? Description { get; set; }
    public int? ValidityPeriod { get; set; }
    public string? ValidityPeriodType { get; set; }
}