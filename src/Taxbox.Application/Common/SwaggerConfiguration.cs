namespace Taxbox.Application.Common;

public class SwaggerConfiguration
{
    public string RoutePrefix { get; init; } = null!;
    public string Version { get; init; } = null!;
    public string SwaggerEndpoint { get; init; } = null!;
}