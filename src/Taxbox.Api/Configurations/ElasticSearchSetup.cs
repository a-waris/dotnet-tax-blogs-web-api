using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
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
        var settings = new ConnectionSettings(new Uri(url)).BasicAuthentication(user, password)
                .CertificateFingerprint(
                    "65:75:8E:22:62:31:47:70:45:8E:6F:8B:FA:46:DF:11:CC:DC:DA:BA:15:9C:27:E3:92:65:15:12:12:72:77:B5")
            // .DefaultIndex(defaultIndex)
            ;

        // AddDefaultMappings(settings);

        var client = new ElasticClient(settings);
        services.AddScoped(typeof(IElasticSearchService<>), typeof(ElasticSearchService<>));
        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);

        return services;
    }

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings
            .DefaultMappingFor<Article>(m => m
                // .Ignore(p => p.Price)
            );
    }

    private static async void CreateIndex(IElasticClient client, string indexName)
    {
        // check if index exists
        if ((await client.Indices.ExistsAsync(indexName)).Exists)
        {
            return;
        }

        var createIndexResponse = await client.Indices.CreateAsync(indexName,
            index => index.Map<Article>(x => x.AutoMap()
                .Properties(p => p
                    .Text(t => t
                        .Name(n => n.Title)
                        .Analyzer("standard")
                    )
                    .Text(t => t
                        .Name(n => n.Metadata)
                    )
                    .Text(t => t
                        .Name(n => n.Content)
                        .Analyzer("english")
                    )
                    .Keyword(k => k
                        .Name(n => n.Author)
                    )
                    .Date(d => d
                        .Name(n => n.Date)
                    )
                    .Keyword(k => k
                        .Name(n => n.Tags)
                    )
                )
            )
        );

        if (!createIndexResponse.IsValid)
        {
            throw new Exception(createIndexResponse.DebugInformation);
        }
    }
}