using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;
using Taxbox.Domain.PaymentGateway.Interfaces;
using Taxbox.Infrastructure.PaymentGateway.Services;

namespace Taxbox.Api.Configurations;

public static class StripeSetup
{
    public static IServiceCollection AddStripeSetup(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        // Add Stripe Services
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<CustomerService>();
        builder.Services.AddScoped<ChargeService>();
        StripeConfiguration.ApiKey = builder.Configuration.GetValue<string>("StripeOptions:SecretKey");

        builder.Services.AddScoped<IStripeService, StripeService>();

        return services;
    }
}