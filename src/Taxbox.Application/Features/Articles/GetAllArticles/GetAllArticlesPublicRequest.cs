using MediatR;
using Newtonsoft.Json;
using Taxbox.Application.Common.Responses;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public record GetAllArticlesPublicRequest : GetAllArticlesRequestBase, IRequest<PaginatedList<GetAllArticlesResponse>>
{
    [JsonIgnore] public override bool? IsPublic { get; set; } = true;
 
}