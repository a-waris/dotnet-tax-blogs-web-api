﻿using Taxbox.Domain.Entities.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Taxbox.Application.Common;

namespace Taxbox.Api.Configurations;

public static class SwaggerSetup
{
    public static IServiceCollection AddSwaggerSetup(this IServiceCollection services, WebApplicationBuilder builder)
    {
        var config = builder.Configuration.GetRequiredSection("SwaggerConfiguration");
        services.Configure<SwaggerConfiguration>(config);

        var appSettings = config.Get<SwaggerConfiguration>();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Taxbox.Api", Version = appSettings?.Version ?? "v1", Description = "API Taxbox"
                });
            c.DescribeAllParametersInCamelCase();
            c.OrderActionsBy(x => x.RelativePath);

            var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
            c.OperationFilter<SecurityRequirementsOperationFilter>();

            // To Enable authorization using Swagger (JWT)    
            c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description =
                    "Enter your valid token in the text input below.\r\n\r\nExample: \"Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9\"",
            });

            // Maps all structured ids to the guid type to show correctly on swagger
            var allGuids = typeof(IGuid).Assembly.GetTypes()
                .Where(type => typeof(IGuid).IsAssignableFrom(type) && !type.IsInterface)
                .ToList();
            foreach (var guid in allGuids)
            {
                c.MapType(guid, () => new OpenApiSchema { Type = "string", Format = "uuid" });
            }
        });
        return services;
    }

    public static IApplicationBuilder UseSwaggerSetup(this IApplicationBuilder app, IConfiguration configuration)
    {
        var config = configuration.GetRequiredSection("SwaggerConfiguration");
        var appSettings = config.Get<SwaggerConfiguration>();

        app.UseSwagger()
            .UseSwaggerUI(c =>
            {
                c.RoutePrefix = appSettings?.RoutePrefix ?? "api-docs";
                c.SwaggerEndpoint(appSettings?.SwaggerEndpoint ?? "/swagger/v1/swagger.json",
                    appSettings?.Version ?? "v1");
                c.DocExpansion(DocExpansion.List);
                c.DisplayRequestDuration();
            });
        return app;
    }
}