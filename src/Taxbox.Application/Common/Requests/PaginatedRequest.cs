namespace Taxbox.Application.Common.Requests;

public record PaginatedRequest
{
    public int CurrentPage { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}