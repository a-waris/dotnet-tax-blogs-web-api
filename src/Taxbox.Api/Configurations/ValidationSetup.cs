using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Taxbox.Api.Configurations;

public static class ValidationSetup
{
    public static void AddValidationSetup(this IMvcBuilder builder)
    {
        builder.Services.AddValidatorsFromAssemblyContaining<Application.IAssemblyMarker>();
    }
}
