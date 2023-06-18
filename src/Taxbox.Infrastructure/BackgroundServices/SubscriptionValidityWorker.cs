using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
    private readonly ILogger<SubscriptionValidityWorker> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public SubscriptionValidityWorker(ILogger<SubscriptionValidityWorker> logger,
        IServiceScopeFactory serviceScopeFactory)
        =>
            (_logger, _serviceScopeFactory) = (logger, serviceScopeFactory);


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Subscription validity worker running at: {Time}", DateTimeOffset.Now);
        while (!stoppingToken.IsCancellationRequested)
        {
            using IServiceScope scope = _serviceScopeFactory.CreateScope();
            try
            {
                _logger.LogInformation(
                    "Starting scoped work, provider hash: {Hash}",
                    scope.ServiceProvider.GetHashCode());
                // get db context from factory
                var context = scope.ServiceProvider.GetRequiredService<IContext>();
                var activeUserSubscriptions = await GetActiveUserSubscriptionsAsync(context, stoppingToken);
                _logger.LogInformation("No. of active subscriptions: {Count}", activeUserSubscriptions.Count);

                foreach (var userSubscription in activeUserSubscriptions.Where(userSubscription =>
                             !IsSubscriptionValid(userSubscription)))
                {
                    _logger.LogInformation("Subscription expired for user: {UserId}", userSubscription.UserId);
                    userSubscription.IsActive = false;
                }

                await context.SaveChangesAsync(stoppingToken);
            }
            finally
            {
                _logger.LogInformation(
                    "Finished scoped work, provider hash: {Hash}.{Nl}",
                    scope.ServiceProvider.GetHashCode(), Environment.NewLine);
                // For testing purposes
                await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);
                // await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        if (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Subscription validity worker stopped at: {Time}", DateTimeOffset.Now);
        }
    }

    private async Task<List<UserSubscription>> GetActiveUserSubscriptionsAsync(IContext context,
        CancellationToken stoppingToken)
    {
        return await context.UserSubscriptions
            .Include(us => us.Subscription)
            .Where(us => us.IsActive)
            .ToListAsync(stoppingToken);
    }

    private static bool IsSubscriptionValid(UserSubscription userSubscription)
    {
        if (userSubscription.Subscription == null)
        {
            return false;
        }

        var isTrial = userSubscription is { TrialStartDate: not null, TrialEndDate: not null };
        var isSub = userSubscription is { SubscriptionStartDate: not null, SubscriptionEndDate: not null };

        // check if subscription is trial and if it has expired
        if (!isSub && isTrial)
        {
            // var expiryDate = CalculateExpiryDate(userSubscription.TrialEndDate!.Value,
            //     userSubscription.Subscription.ValidityPeriod,
            //     userSubscription.Subscription.ValidityPeriodType);
            // return DateTime.Now < expiryDate;
            return DateTime.UtcNow < userSubscription.TrialEndDate!.Value;
        }
        else
        {
            // var expiryDate = CalculateExpiryDate(userSubscription.SubscriptionEndDate.Value,
            //     userSubscription.Subscription.ValidityPeriod,
            //     userSubscription.Subscription.ValidityPeriodType);
            // return DateTime.Now < expiryDate;
            return DateTime.UtcNow < userSubscription.SubscriptionEndDate!.Value;
        }
    }

    // private static DateTime CalculateExpiryDate(DateTime startDate, int validityPeriod, string validityPeriodType)
    // {
    //     return validityPeriodType.ToLower() switch
    //     {
    //         "days" => startDate.AddDays(validityPeriod),
    //         "months" => startDate.AddMonths(validityPeriod),
    //         _ => throw new ArgumentException("Invalid validity period type.")
    //     };
    // }
}