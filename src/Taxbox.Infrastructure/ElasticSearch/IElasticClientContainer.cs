using Elastic.Clients.Elasticsearch;

namespace Taxbox.Infrastructure.ElasticSearch;

public interface IElasticClientContainer
{
    ElasticsearchClient GetElasticClient();
}