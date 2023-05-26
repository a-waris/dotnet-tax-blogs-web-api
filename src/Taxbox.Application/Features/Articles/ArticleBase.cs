using Taxbox.Domain.ElasticSearch.Interfaces;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Articles;

public record ArticleBase()
{
    private readonly IElasticSearchService<Article>? _eSservice;

    public ArticleBase(IElasticSearchService<Article>? eSservice) : this()
    {
        _eSservice = eSservice;
    }
}