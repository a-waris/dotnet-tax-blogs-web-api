using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
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
            // Get all active user subscriptions
            var activeUserSubscriptions = await _context.UserSubscriptions
                .Include(us => us.Subscription)
                .Where(us => us.IsActive)
                .ToListAsync(cancellationToken: cancellationToken);

            foreach (var userSubscription in activeUserSubscriptions.Where(userSubscription =>
                         !IsSubscriptionValid(userSubscription.Subscription, userSubscription.SubscriptionEndDate)))
            {
                // Subscription is no longer valid, set IsActive to false
                userSubscription.IsActive = false;
            }

            await _context.SaveChangesAsync(cancellationToken);
            await Task.Delay(TimeSpan.FromDays(1), cancellationToken); // Check validity daily
        }
    }

    private bool IsSubscriptionValid(Subscription? subscription, DateTime? subscriptionDate)
    {
        if (subscription != null)
        {
            var expiryDate =
                CalculateExpiryDate(subscriptionDate, subscription.ValidityPeriod, subscription.ValidityPeriodType);
            return DateTime.Now < expiryDate;
        }

        return false;
    }

    private DateTime? CalculateExpiryDate(DateTime? startDate, int validityPeriod, string validityPeriodType)
    {
        if (startDate != null)
        {
            return validityPeriodType.ToLower() switch
            {
                "days" => startDate.Value.AddDays(validityPeriod),
                "months" => startDate.Value.AddMonths(validityPeriod),
                _ => throw new ArgumentException("Invalid validity period type.")
            };
        }

        _logger.LogError("Subscription start date is null.");
        return null;
    }
}