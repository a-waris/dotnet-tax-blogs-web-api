namespace Taxbox.Application.Common;

public class ElasticSearchConfiguration
{
    public string Url { get; init; } = null!;
    public string ArticlesIndex { get; init; } = null!;
    public bool UseLocalEs { get; init; } = false;
    public string AuthorsIndex { get; init; } = null!;
    public string User { get; init; } = null!;
    public string Password { get; init; } = null!;
    public bool EnableDebugMode { get; set; }
    public string ClientCertificatePath { get; set; } = null!;
    public string CloudId { get; set; } = null!;
}