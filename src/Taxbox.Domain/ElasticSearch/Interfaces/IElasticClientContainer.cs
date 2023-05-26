using Elastic.Clients.Elasticsearch;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IElasticClientContainer
{
    ElasticsearchClient GetElasticClient();
    string GetIndexName();
}