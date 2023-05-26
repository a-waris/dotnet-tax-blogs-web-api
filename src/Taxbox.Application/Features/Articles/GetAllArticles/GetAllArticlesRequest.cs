using MediatR;
using System;
using System.Collections.Generic;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities;
using Taxbox.Domain.Entities.Common;

namespace Taxbox.Application.Features.Articles.GetAllArticles;

public class GetAllArticlesRequest : IRequest<IList<GetArticleResponse>>
{
    public string? Title { get; set; }
    public Metadata? Metadata { get; set; }
    public string? Content { get; set; }
    public string? Author { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IList<string>? Tags { get; set; }
}