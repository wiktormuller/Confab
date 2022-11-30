using System.Runtime.CompilerServices;
using Confab.Modules.Conferences.Core;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Bootstrapper")] // Now, it's visible to Bootstraper project ONLY
namespace Confab.Modules.Conferences.Api;

internal static class Extensions
{
    internal static IServiceCollection AddConferences(this IServiceCollection services)
    {
        services.AddCore();
        return services; // Fluent
    }
}