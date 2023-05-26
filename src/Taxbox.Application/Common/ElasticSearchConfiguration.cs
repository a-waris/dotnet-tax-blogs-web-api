namespace Taxbox.Application.Common;

public class ElasticSearchConfiguration
{
    public string Url { get; init; } = null!;
    public string DefaultIndex { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
    public bool EnableDebugMode { get; set; }
}