using Elastic.Clients.Elasticsearch;

namespace Taxbox.Infrastructure.ElasticSearch;

public class ElasticClientContainer : IElasticClientContainer
{
    private ElasticsearchClient _client;

    public ElasticClientContainer(IElasticsearchClientSettings settings)
    {
        _client = new ElasticsearchClient(settings);
    }

    public ElasticsearchClient GetElasticClient()
    {
        return _client;
    }
}