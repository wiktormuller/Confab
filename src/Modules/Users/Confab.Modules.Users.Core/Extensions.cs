
using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Modules.Users.Api")] // Now, it's visible to Api project ONLY
namespace Confab.Modules.Users.Core;

internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        return services;
    }
}