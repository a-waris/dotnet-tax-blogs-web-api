using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxbox.Infrastructure.ElasticSearch;

public interface IElasticSearchService<T> where T : class
{
    Task CreateIndexIfNotExists(string indexName);
    Task<bool> AddOrUpdateBulk<T>(IEnumerable<T> documents) where T : class;
    Task<bool> AddOrUpdate<T>(T document) where T : class;
    Task<T> Get<T>(string key) where T : class;
    Task<List<T>?> GetAll<T>() where T : class;
    Task<List<T>?> Query<T>(QueryDescriptor<T> predicate) where T : class;
    Task<bool> Remove<T>(string key) where T : class;
    Task<long> RemoveAll<T>() where T : class;
    Task<bool> RemoveIndex(string indexName);
    Task<bool> IndexExists(string indexName);
    Task<bool> AliasExists(string aliasName);
    Task<bool> AddAlias(string aliasName, string indexName);
    Task<bool> RemoveAlias(string aliasName, string indexName);
    Task<bool> ReIndex(string sourceIndexName, string destinationIndexName);

    Task<List<T>?> GetAllPaginated<T>(QueryDescriptor<T> predicate, int currentPage, int pageSize)
        where T : class;
}