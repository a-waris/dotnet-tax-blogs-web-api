using Nest;
using Taxbox.Domain.ElasticSearch.Interfaces;

namespace Taxbox.Infrastructure.ElasticSearch;

public class ElasticClientContainer : IElasticClientContainer
{
    private readonly ElasticClient _client;
    private readonly string _indexName;

    public ElasticClientContainer(ElasticClient client, string indexName)
    {
        _client = client;
        _indexName = indexName;
    }

    public ElasticClient GetElasticClient()
    {
        return _client;
    }

    public string GetIndexName()
    {
        return _indexName;
    }
}