using Amazon.S3;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Taxbox.Application.Common;

namespace Taxbox.Api.Configurations;

public static class SetupS3Client
{
    public static IServiceCollection AddS3Setup(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        var configs = builder.Configuration.GetRequiredSection("AWSConfiguration");
        services.Configure<AWSConfiguration>(configs);

        var appSettings = configs.Get<AWSConfiguration>();

        if (appSettings == null)
        {
            Console.Write("AWSConfiguration is not configured");
            return services;
        }

        var client = new AmazonS3Client(appSettings.AccessKey, appSettings.SecretKey,
            Amazon.RegionEndpoint.GetBySystemName(appSettings.Region));

        services.AddSingleton<IAmazonS3>(client);
        return services;
    }
}