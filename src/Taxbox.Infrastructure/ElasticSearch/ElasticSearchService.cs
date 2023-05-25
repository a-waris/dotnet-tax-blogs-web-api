using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Taxbox.Infrastructure.ElasticSearch;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
{
    private string _indexName { get; set; }
    private readonly ElasticsearchClient _client;

    public ElasticSearchService(IElasticClientContainer clientContainer, string indexName)
    {
        _client = clientContainer.GetElasticClient();
        _indexName = indexName;
    }

    public ElasticSearchService<T> Index(string indexName)
    {
        _indexName = indexName;
        return this;
    }

    public async Task CreateIndexIfNotExists(string indexName)
    {
        if (!(await _client.Indices.ExistsAsync(indexName)).Exists)
        {
            await _client.Indices.CreateAsync(indexName,
                c => c.Mappings(descriptor => descriptor.Dynamic(DynamicMapping.True)));
        }

        Index(indexName);
    }

    public async Task<bool> AddOrUpdateBulk<T>(IEnumerable<T> documents) where T : class
    {
        var indexResponse = await _client.BulkAsync(b => b
            .Index(_indexName)
            .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert())
        );
        return indexResponse.IsValidResponse;
    }

    public async Task<bool> AddOrUpdate<T>(T document) where T : class
    {
        var indexResponse =
            await _client.IndexAsync(document, idx => idx.Index(_indexName).OpType(OpType.Index));
        return indexResponse.IsValidResponse;
    }

    public async Task<T> Get<T>(string key) where T : class
    {
        var response = await _client.GetAsync<T>(key, g => g.Index(_indexName));
        return response.Source;
    }

    public async Task<List<T>?> GetAll<T>() where T : class
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(q => q.MatchAll()));
        return searchResponse.IsValidResponse ? searchResponse.Documents.ToList() : default;
    }

    public async Task<List<T>?> GetAllPaginated<T>(QueryDescriptor<T> predicate, int currentPage, int pageSize)
        where T : class
    {
        // return await searchRequest.ToPaginatedListAsync(_client, currentPage, pageSize);
        var searchResponse = await _client.SearchAsync<T>(s =>
            s.Index(_indexName).From((currentPage - 1) * pageSize).Size(pageSize).Query(predicate));
        return searchResponse.IsValidResponse ? searchResponse.Documents.ToList() : default;
    }

    public async Task<List<T>?> Query<T>(QueryDescriptor<T> predicate) where T : class
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(predicate));
        return searchResponse.IsValidResponse ? searchResponse.Documents.ToList() : default;
    }

    public async Task<bool> Remove<T>(string key) where T : class
    {
        var response = await _client.DeleteAsync<T>(key, d => d.Index(_indexName));
        return response.IsValidResponse;
    }

    public async Task<long> RemoveAll<T>() where T : class
    {
        var response = await _client.DeleteByQueryAsync<T>(_indexName, q => q.Query(
            qd => qd.MatchAll()
        ));

        return (long)(response.IsValidResponse ? response.Deleted! : 0);
    }

    public Task<bool> RemoveIndex(string indexName)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> IndexExists(string indexName)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> AliasExists(string aliasName)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> AddAlias(string aliasName, string indexName)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> RemoveAlias(string aliasName, string indexName)
    {
        throw new System.NotImplementedException();
    }

    public Task<bool> ReIndex(string sourceIndexName, string destinationIndexName)
    {
        throw new System.NotImplementedException();
    }
}