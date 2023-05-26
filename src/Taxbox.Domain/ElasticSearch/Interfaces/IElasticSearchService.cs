using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IElasticSearchService<T> where T : class
{
    Task CreateIndexIfNotExists(string indexName);
    Task<bool> AddOrUpdateBulk(IEnumerable<T> documents);
    // Task<bool> AddOrUpdate(T document);
    Task<T> AddOrUpdate(T document);
    Task<T?> Get(string key);
    Task<List<T>?> GetAll();
    Task<List<T>?> Query(QueryDescriptor<T> predicate);
    Task<bool> Remove(string key);
    Task<long> RemoveAll();
    Task<bool> RemoveIndex(string indexName);
    Task<bool> IndexExists(string indexName);
    Task<bool> AliasExists(string aliasName);
    Task<bool> AddAlias(string aliasName, string indexName);
    Task<bool> RemoveAlias(string aliasName, string indexName);
    Task<bool> ReIndex(string sourceIndexName, string destinationIndexName);

    Task<SearchResponse<T>?> GetAllPaginated(QueryDescriptor<T> predicate, int currentPage, int pageSize);
}