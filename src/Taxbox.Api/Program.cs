using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Taxbox.Api.Common;
using Taxbox.Api.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json.Serialization;
using Taxbox.Infrastructure.BackgroundServices;

var builder = WebApplication.CreateBuilder(args);

// Controllers
builder.Services
    .AddControllers(options =>
    {
        options.AllowEmptyInputInBodyModelBinding = true;
        options.AddResultConvention(resultMap =>
        {
            resultMap.AddDefaultMap()
                .For(ResultStatus.Ok, HttpStatusCode.OK, resultStatusOptions => resultStatusOptions
                    .For("POST", HttpStatusCode.Created)
                    .For("DELETE", HttpStatusCode.NoContent));
        });
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.WriteIndented = true;
    })
    .AddValidationSetup();

// Authn / Authrz
builder.Services.AddAuthSetup(builder.Configuration);

// Swagger
builder.Services.AddSwaggerSetup(builder);

// ElasticSearch
builder.Services.AddElasticSearchSetup(builder);

// Persistence
builder.Services.AddPersistenceSetup(builder.Configuration);

// Application layer setup
builder.Services.AddApplicationSetup();

// Request response compression
builder.Services.AddCompressionSetup();

// HttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Mediator
builder.Services.AddMediatRSetup();

// Middleware
builder.Services.AddScoped<ExceptionHandlerMiddleware>();

builder.Logging.ClearProviders();

// Add serilog
if (builder.Environment.EnvironmentName != "Testing")
{
    builder.Host.UseLoggingSetup(builder.Configuration);
}

// Add opentelemetry
builder.AddOpenTemeletrySetup();

// Add S3 client
builder.Services.AddS3Setup(builder);

// Add Stripe Services
builder.Services.AddStripeSetup(builder);

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        corsPolicyBuilder =>
        {
            corsPolicyBuilder.WithOrigins(
                "*"
                // TODO: LIVE https URL OF FRONT END HERE
            );
            corsPolicyBuilder.AllowAnyHeader();
            corsPolicyBuilder.AllowAnyMethod();
        });
});

if (builder.Configuration.GetSection("ServiceWorkerConfiguration").GetValue<bool>("IsEnabled"))
{
    // Add Background Service Workers
    builder.Services.AddHostedService<SubscriptionValidityWorker>();
}

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseResponseCompression();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseCors("AllowSpecificOrigins");

app.UseMiddleware(typeof(ExceptionHandlerMiddleware));

app.UseSwaggerSetup(builder.Configuration);

app.UseResponseCompression();
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers()
    .RequireAuthorization();

await app.Migrate();

await app.RunAsync();