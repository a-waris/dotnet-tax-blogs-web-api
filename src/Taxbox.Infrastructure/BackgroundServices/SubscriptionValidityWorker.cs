using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;

namespace Taxbox.Infrastructure.BackgroundServices;

public class SubscriptionValidityWorker : BackgroundService
{
    private readonly IContext _context;
    private readonly ILogger<SubscriptionValidityWorker> _logger;

    public SubscriptionValidityWorker(IContext context, ILogger<SubscriptionValidityWorker> logger)
    {
        _context = context;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var activeUserSubscriptions = await GetActiveUserSubscriptionsAsync(cancellationToken);

            foreach (var userSubscription in activeUserSubscriptions)
            {
                if (!IsSubscriptionValid(userSubscription.Subscription, userSubscription.SubscriptionEndDate))
                {
                    userSubscription.IsActive = false;
                }
            }

            await _context.SaveChangesAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromDays(1), cancellationToken);
        }
    }

    private async Task<List<UserSubscription>> GetActiveUserSubscriptionsAsync(CancellationToken cancellationToken)
    {
        return await _context.UserSubscriptions
            .Include(us => us.Subscription)
            .Where(us => us.IsActive)
            .ToListAsync(cancellationToken);
    }

    private static bool IsSubscriptionValid(Subscription? subscription, DateTime? subscriptionDate)
    {
        if (subscription == null || subscriptionDate == null)
        {
            return false;
        }

        var expiryDate = CalculateExpiryDate(subscriptionDate.Value, subscription.ValidityPeriod,
            subscription.ValidityPeriodType);
        return DateTime.Now < expiryDate;

    }

    private static DateTime CalculateExpiryDate(DateTime startDate, int validityPeriod, string validityPeriodType)
    {
        return validityPeriodType.ToLower() switch
        {
            "days" => startDate.AddDays(validityPeriod),
            "months" => startDate.AddMonths(validityPeriod),
            _ => throw new ArgumentException("Invalid validity period type.")
        };
    }
}