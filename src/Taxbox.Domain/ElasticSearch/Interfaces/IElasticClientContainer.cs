using Nest;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IElasticClientContainer
{
    ElasticClient GetElasticClient();
    string GetIndexName();
}