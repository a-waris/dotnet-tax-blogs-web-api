using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Infrastructure.ElasticSearch;

public class ElasticSearchService<T> : IElasticSearchService<T> where T : class
{
    private string IndexName { get; set; }
    private readonly IElasticClient _client;

    public ElasticSearchService(IElasticClient client)
    {
        _client = client;
        IndexName = typeof(T).Name.ToLower() + "s";
    }

    public IElasticSearchService<T> Index(string indexName)
    {
        IndexName = indexName;
        return this;
    }

    public async Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents)
    {
        var indexResponse = await _client.BulkAsync(b => b
            .Index(IndexName)
            .UpdateMany(documents, (ud, d) => ud.Doc(d).DocAsUpsert())
        );
        return indexResponse;
    }

    public async Task<T> AddOrUpdate(T document)
    {
        var indexResponse =
            await _client.IndexAsync(document, idx => idx.Index(IndexName));
        if (!indexResponse.IsValid)
        {
            throw new Exception(indexResponse.DebugInformation);
        }

        return document;
    }

    public async Task<BulkResponse> AddBulk(IList<T> documents)
    {
        var resp = await _client.BulkAsync(b => b
            .Index(IndexName)
            .IndexMany(documents)
        );
        return resp;
    }

    public async Task<GetResponse<T>> Get(string key)
    {
        return await _client.GetAsync<T>(key, g => g.Index(IndexName));
    }

    public async Task<List<T>?> GetAll()
    {
        var searchResponse = await _client.SearchAsync<T>(s => s.Index(IndexName).Query(q => q.MatchAll()));
        return searchResponse.IsValid ? searchResponse.Documents.ToList() : default;
    }

    public async Task<ISearchResponse<T>?> Query(SearchDescriptor<T> sd)
    {
        var searchResponse = await _client.SearchAsync<T>(sd);
        return searchResponse;
    }

    public async Task<bool> Remove(string key)
    {
        var response = await _client.DeleteAsync<T>(key, d => d.Index(IndexName));
        return response.IsValid;
    }

    public async Task<DeleteByQueryResponse> BulkRemove(IDeleteByQueryRequest<T> queryReq)
    {
        var response = await _client.DeleteByQueryAsync(queryReq);
        return response;
    }
}