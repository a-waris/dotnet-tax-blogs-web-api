using MediatR;
using Taxbox.Application.Common.Responses;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public record GetAllArticlesRequest : GetAllArticlesRequestBase, IRequest<PaginatedList<GetAllArticlesResponse>>
{
   
}