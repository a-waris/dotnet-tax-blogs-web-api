using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IElasticSearchService<T> where T : class
{
    IElasticSearchService<T> Index(string indexName);
    Task CreateIndexIfNotExists(string indexName);
    Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents);
    // Task<bool> AddOrUpdate(T document);
    Task<T> AddOrUpdate(T document);
    Task<BulkResponse> AddBulk(IList<T> documents);
    Task<GetResponse<T>> Get(string key);
    Task<List<T>?> GetAll();
    Task<SearchResponse<T>> Query(QueryDescriptor<T> predicate);
    Task<SearchResponse<T>?> Query(SearchRequestDescriptor<T> searchRequestDescriptor);
    Task<bool> Remove(string key);
    Task<long> RemoveAll();
    Task<bool> RemoveIndex(string indexName);
    Task<bool> IndexExists(string indexName);
    Task<bool> AliasExists(string aliasName);
    Task<bool> AddAlias(string aliasName, string indexName);
    Task<bool> RemoveAlias(string aliasName, string indexName);
    Task<bool> ReIndex(string sourceIndexName, string destinationIndexName);

    Task<SearchResponse<T>?> GetAllPaginated(QueryDescriptor<T> predicate, int currentPage, int pageSize,
        string[]? sourceFields = null, SortOptionsDescriptor<T>? sortDescriptor = null);
}