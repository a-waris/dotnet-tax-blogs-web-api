using Elastic.Clients.Elasticsearch;
using Taxbox.Domain.ElasticSearch.Interfaces;

namespace Taxbox.Infrastructure.ElasticSearch;

public class ElasticClientContainer : IElasticClientContainer
{
    private ElasticsearchClient _client;
    private string _indexName;

    public ElasticClientContainer(IElasticsearchClientSettings settings, string indexName)
    {
        _client = new ElasticsearchClient(settings);
        _indexName = indexName;
    }

    public ElasticsearchClient GetElasticClient()
    {
        return _client;
    }

    public string GetIndexName()
    {
        return _indexName;
    }
}