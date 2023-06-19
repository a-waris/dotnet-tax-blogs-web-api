using Nest;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Taxbox.Domain.ElasticSearch.Interfaces;

public interface IElasticSearchService<T> where T : class
{
    IElasticSearchService<T> Index(string indexName);
    Task<BulkResponse> AddOrUpdateBulk(IEnumerable<T> documents);
    Task<T> AddOrUpdate(T document);
    Task<BulkResponse> AddBulk(IList<T> documents);
    Task<GetResponse<T>> Get(string key);
    Task<ISearchResponse<T>?> Query(SearchDescriptor<T> sd);
    Task<bool> Remove(string key);
    Task<DeleteByQueryResponse> BulkRemove(IDeleteByQueryRequest<T> queryReq);
}