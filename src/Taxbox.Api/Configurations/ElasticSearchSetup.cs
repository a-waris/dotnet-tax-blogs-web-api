using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;
using Taxbox.Infrastructure.ElasticSearch;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Taxbox.Domain.ElasticSearch.Interfaces;

namespace Taxbox.Api.Configurations;

public static class ElasticSearchSetup
{
    public static IServiceCollection AddElasticSearchSetup(this IServiceCollection services,
        IConfiguration configuration)
    {
        var esConfig = configuration.GetRequiredSection("ElasticSearchConfiguration");
        services.Configure<ElasticSearchConfiguration>(esConfig);

        var appSettings = esConfig.Get<ElasticSearchConfiguration>();

        if (appSettings == null)
        {
            throw new Exception("ElasticSearchConfiguration is not configured");
        }

        var defaultIndex = appSettings.DefaultIndex;
        var debugMode = appSettings.EnableDebugMode;
        var settings = EsSettings(appSettings);
        if (debugMode)
        {
            settings.EnableDebugMode();
        }

        // AddDefaultMappings(settings, defaultIndex);

        var client = new ElasticClientContainer(settings, defaultIndex);
        services.AddSingleton<IElasticsearchClientSettings>(settings);
        services.AddSingleton<IElasticClientContainer>(client);
        services.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));

        CreateIndex(client, defaultIndex);

        return services;
    }

    private static ElasticsearchClientSettings EsSettings(ElasticSearchConfiguration esConfig)
    {
        var isProduction = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Production";
        var auth = new BasicAuthentication(esConfig.User, esConfig.Password);
        if (isProduction)
        {
            return new ElasticsearchClientSettings(esConfig.CloudId, auth)
                .DefaultIndex(esConfig.DefaultIndex);
        }

        var nodes = new Uri[] { new(esConfig.Url) };
        var pool = new StaticNodePool(nodes);
        return new ElasticsearchClientSettings(pool)
            .CertificateFingerprint(GetSha2Thumbprint(new X509Certificate2(esConfig.ClientCertificatePath)))
            .Authentication(auth)
            .DefaultIndex(esConfig.DefaultIndex);
    }

    private static void AddDefaultMappings(ElasticsearchClientSettings settings, string defaultIndex)
    {
        settings
            .DefaultMappingFor<Article>
            (i => i
                .IndexName(defaultIndex)
                .IdProperty(a => a.Id)
            )
            .PrettyJson();
    }

    private static async void CreateIndex(IElasticClientContainer clientContainer, string indexName)
    {
        var client = clientContainer.GetElasticClient();
        var indexExists = await client.Indices.ExistsAsync(indexName);
        if (!indexExists.IsValidResponse)
        {
            throw new Exception(indexExists.DebugInformation);
        }

        if (indexExists.Exists)
        {
            return;
        }

        var createIndexResponse = await client.Indices.CreateAsync(indexName,
            index => index.Mappings(descriptor =>
                descriptor.Properties<Article>(p => p
                    .Text(t => t.Title)
                    .Object(o => o.Metadata!)
                    .Text(t => t.Content!)
                    .Keyword(k => k.Author!)
                    .Date(d => d.CreatedAt!)
                    .Date(d => d.UpdatedAt!)
                    .Keyword(k => k.Tags!)
                )
            ).Settings(descriptor =>
                descriptor.Analysis(analysis =>
                    analysis
                        .Analyzers(analyzers => analyzers
                            .Standard("standard")
                        )
                ))
        );

        if (!createIndexResponse.IsValidResponse)
        {
            throw new Exception(createIndexResponse.DebugInformation);
        }
    }

    private static string GetSha2Thumbprint(X509Certificate2 cert)
    {
        var hash = SHA256.Create().ComputeHash(cert.RawData);
        return BitConverter.ToString(hash).Replace("-", ":");
    }
}