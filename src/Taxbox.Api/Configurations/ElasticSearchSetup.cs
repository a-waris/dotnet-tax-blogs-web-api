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

        var url = appSettings!.Url;
        var defaultIndex = appSettings.DefaultIndex;
        var user = appSettings.User;
        var password = appSettings.Password;
        var debugMode = appSettings.EnableDebugMode;
        var clientCertificatePath = appSettings.ClientCertificatePath;
        var nodes = new Uri[]
        {
            new(url),
            // new Uri("https://myserver2:9200"), 
            // new Uri("https://myserver3:9200")
        };
        var pool = new StaticNodePool(nodes);
        var settings = new ElasticsearchClientSettings(pool)
                // .CertificateFingerprint(
                //     "E2:E6:65:B8:F9:CB:C7:39:2D:8A:2B:9A:35:C3:68:8B:AD:B5:E6:2D:BE:21:8A:BF:71:15:25:77:A6:0D:A2:22"
                // )
                // .ServerCertificateValidationCallback(CertificateValidations.AllowAll)
                .CertificateFingerprint(GetSha2Thumbprint(new X509Certificate2(clientCertificatePath)))
                .Authentication(new BasicAuthentication(user, password))
                .DefaultIndex(defaultIndex)
            ;
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
        // check if index exists
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