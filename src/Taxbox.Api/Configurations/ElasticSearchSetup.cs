using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using System;
using Taxbox.Application.Common;
using Taxbox.Domain.Entities;
using Taxbox.Infrastructure.ElasticSearch;

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
        var settings = new ElasticsearchClientSettings(new Uri(url))
        .CertificateFingerprint("65:75:8E:22:62:31:47:70:45:8E:6F:8B:FA:46:DF:11:CC:DC:DA:BA:15:9C:27:E3:92:65:15:12:12:72:77:B5")
                .Authentication(new BasicAuthentication(user, password))
            // .ClientCertificate("C:\\Users\\waris\\source\\repos\\taxbox-api\\certs\\es01.crt")
            // .DefaultIndex(defaultIndex)
            ;

        // AddDefaultMappings(settings, defaultIndex);

        var client = new ElasticClientContainer(settings);
        services.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));
        services.AddSingleton<IElasticsearchClientSettings>(settings);
        services.AddSingleton<IElasticClientContainer>(client);

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
            // .EnableDebugMode()
            .PrettyJson()
            // .RequestTimeout(TimeSpan.FromMinutes(2))
            ;
    }

    private static async void CreateIndex(IElasticClientContainer clientContainer, string indexName)
    {
        var client = clientContainer.GetElasticClient();
        var a = await client.Indices.ExistsAsync(indexName);
        // check if index exists
        if ((await client.Indices.ExistsAsync(indexName)).Exists)
        {
            return;
        }

        var createIndexResponse = await client.Indices.CreateAsync(indexName,
            index => index.Mappings(descriptor => descriptor.Properties<Article>(p => p
                    .Text(t => t.Title)
                    .Text(t => t.Metadata!)
                    .Text(t => t.Content!)
                    .Keyword(k => k.Author!)
                    .Date(d => d.Date!)
                    .Keyword(k => k.Tags!)
                )
            )
        );

        if (!createIndexResponse.IsValidResponse)
        {
            throw new Exception(createIndexResponse.DebugInformation);
        }
    }
}