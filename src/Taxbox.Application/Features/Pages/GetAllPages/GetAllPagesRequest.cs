using MediatR;
using System;
using Taxbox.Application.Common.Requests;
using Taxbox.Application.Common.Responses;
using Taxbox.Domain.Entities;

namespace Taxbox.Application.Features.Pages.GetAllPages;

public record GetAllPagesRequest : PaginatedRequest, IRequest<PaginatedList<GetPageResponse>>
{
    public string? Label { get; set; } = null!;
    public string? HtmlContent { get; set; } = null!;
    public Metadata? Metadata { get; set; }
    public string? Slug { get; set; }
    public string? ParentName { get; set; }
    
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}