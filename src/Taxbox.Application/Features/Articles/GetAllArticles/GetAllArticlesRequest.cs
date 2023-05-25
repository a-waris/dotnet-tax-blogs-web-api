using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public class GetAllArticlesRequest : IRequest<PaginatedList<GetArticleResponse>>, IRequest<IList<GetArticleResponse>>
{
    public string? Title { get; set; }
    public string? Metadata { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime? Date { get; set; }
    public IList<string>? Tags { get; set; }
}