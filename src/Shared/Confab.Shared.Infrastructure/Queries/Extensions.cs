using Confab.Shared.Abstractions.Queries;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Confab.Shared.Infrastructure.Queries;

internal static class Extensions
{
    internal static IServiceCollection AddQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();
        services.Scan(s => s.FromAssemblies(assemblies) // With help of scrutor package register all query handlers
            .AddClasses(c => c
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
