using Mapster;
using Taxbox.Application.Features.Articles.GetAllArticles;
using Taxbox.Application.Features.Articles.UpdateArticle;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.MappingConfig;

public class ArticleMappingConfig : IMappingConfig
{
    public void ApplyConfig()
    {
        TypeAdapterConfig<GetAllArticlesRequest, Article>
            .ForType()
            .Map(dest => dest.Id,
                opt => opt);

        TypeAdapterConfig<UpdateArticleRequest, Article>
            .NewConfig()
            .IgnoreNullValues(true);
    }
}