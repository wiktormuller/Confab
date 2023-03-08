using Microsoft.Extensions.DependencyInjection;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Confab.Modules.Attendances.Tests.Unit")]
[assembly: InternalsVisibleTo("DynamicProxyAssemblyGen2")]
namespace Confab.Modules.Attendances.Application;

public static class Extensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services;
    }
}
