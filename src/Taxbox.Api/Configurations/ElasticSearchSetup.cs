using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elasticsearch.Net;
using Microsoft.AspNetCore.Builder;
using Nest;
using System;
using Taxbox.Application.Common;
using Taxbox.Infrastructure.ElasticSearch;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Api.Configurations;

public static class ElasticSearchSetup
{
    public static IServiceCollection AddElasticSearchSetup(this IServiceCollection services,
        WebApplicationBuilder builder)
    {
        var esConfig = builder.Configuration.GetRequiredSection("ElasticSearchConfiguration");
        services.Configure<ElasticSearchConfiguration>(esConfig);

        var appSettings = esConfig.Get<ElasticSearchConfiguration>();

        if (appSettings == null)
        {
            throw new Exception("ElasticSearchConfiguration is not configured");
        }

        var index = appSettings.ArticlesIndex;
        var esClient = ElasticClient(appSettings);

        services.AddSingleton<IElasticClient>(esClient);
        services.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));

        return services;
    }

    private static ElasticClient ElasticClient(ElasticSearchConfiguration esConfig)
    {
        // var auth = new BasicAuthentication(esConfig.User, esConfig.Password);
        var auth = new BasicAuthenticationCredentials(esConfig.User, esConfig.Password);
        // if (esConfig.UseLocalEs)
        // {
        //     var nodes = new Uri[] { new(esConfig.Url) };
        //     var pool = new StaticNodePool(nodes);
        //     return new ElasticsearchClientSettings(pool)
        //         .CertificateFingerprint(GetSha2Thumbprint(new X509Certificate2(esConfig.ClientCertificatePath)))
        //         .Authentication(auth)
        //         .DefaultIndex(esConfig.ArticlesIndex);
        // }

        var connectionSettings = new ConnectionSettings(esConfig.CloudId, auth)
            .DefaultIndex(esConfig.ArticlesIndex)
            .DefaultMappingFor<Article>(i => i
                .IndexName(esConfig.ArticlesIndex)
                .IdProperty(p => p.Id)
            )
            .DefaultMappingFor<Author>(i => i
                .IndexName(esConfig.AuthorsIndex)
                .IdProperty(p => p.Id))
            .EnableApiVersioningHeader()
            .DisableDirectStreaming()
            .RequestTimeout(TimeSpan.FromMinutes(2));
        if (esConfig.EnableDebugMode)
        {
            // connectionSettings.EnableDebugMode();
        }

        return new ElasticClient(connectionSettings);

        // return new ElasticsearchClientSettings(esConfig.CloudId, auth)
        //     .DefaultIndex(esConfig.ArticlesIndex);
    }

    private static async void IndexExistsCheck(IElasticClient client, string indexName)
    {
        var indexExists = await client.Indices.ExistsAsync(indexName);

        if (!indexExists.Exists)
        {
            throw new Exception("Index does not exist");
        }
    }

    // private static async void CreateIndex(IElasticClientContainer clientContainer, string indexName)
    // {
    //     var client = clientContainer.GetElasticClient();
    //     var indexExists = await client.Indices.ExistsAsync(indexName);
    //
    //     if (!indexExists.Exists)
    //     {
    //         throw new Exception("Index does not exist");
    //     }
    // }


    private static string GetSha2Thumbprint(X509Certificate2 cert)
    {
        var hash = SHA256.Create().ComputeHash(cert.RawData);
        return BitConverter.ToString(hash).Replace("-", ":");
    }
}