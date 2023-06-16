using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Mapping;
using Elastic.Clients.Elasticsearch.QueryDsl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Infrastructure.ElasticSearch;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
{
    private string _indexName { get; set; }
    private readonly ElasticsearchClient _client;

    public ElasticSearchService(IElasticClientContainer clientContainer)
    {
        _client = clientContainer.GetElasticClient();
        _indexName = clientContainer.GetIndexName();
    }

    public IElasticSearchService<T> Index(string indexName)
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

    public async Task<bool> AddOrUpdateBulk(IEnumerable<T> documents)
    {
        var indexResponse = await _client.BulkAsync(b => b
            .Index(_indexName)
            .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert())
        );
        return indexResponse.IsValidResponse;
    }

    // public async Task<bool> AddOrUpdate(T document)
    // {
    //     var indexResponse =
    //         await _client.IndexAsync(document, idx => idx.Index(_indexName));
    //     return indexResponse.IsValidResponse;
    // }

    public async Task<T> AddOrUpdate(T document)
    {
        var indexResponse =
            await _client.IndexAsync(document, idx => idx.Index(_indexName));
        if (!indexResponse.IsValidResponse)
        {
            throw new Exception(indexResponse.DebugInformation);
        }

        return document;
    }

    public async Task<GetResponse<T>> Get(string key)
    {
        return await _client.GetAsync<T>(key, g => g.Index(_indexName));
    }

    public async Task<List<T>?> GetAll()
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(q => q.MatchAll()));
        return searchResponse.IsValidResponse ? searchResponse.Documents.ToList() : default;
    }

    public async Task<SearchResponse<T>?> GetAllPaginated(QueryDescriptor<T> predicate, int currentPage, int pageSize,
        string[]? sourceFields = null, SortOptionsDescriptor<T>? sortDescriptor = null)

    {
        var searchRequestDescriptor = new SearchRequestDescriptor<T>();
        searchRequestDescriptor.Query(predicate);
        searchRequestDescriptor.Index(_indexName);
        searchRequestDescriptor.From((currentPage - 1) * pageSize);
        searchRequestDescriptor.Size(pageSize);

        if (sortDescriptor != null)
        {
            searchRequestDescriptor.Sort(sortDescriptor);
        }


        if (sourceFields != null && sourceFields.Any())
        {
            searchRequestDescriptor.SourceIncludes(sourceFields);
        }

        var searchResponse = await _client.SearchAsync(searchRequestDescriptor);
        return searchResponse.IsValidResponse ? searchResponse : default;
    }

    public async Task<SearchResponse<T>> Query(QueryDescriptor<T> predicate)
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(_indexName).Query(predicate));
        return searchResponse;
    }

    public async Task<bool> Remove(string key)
    {
        var response = await _client.DeleteAsync<T>(key, d => d.Index(_indexName));
        return response.IsValidResponse;
    }

    public async Task<long> RemoveAll()
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