using Confab.Shared.Abstractions.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Confab.Shared.Infrastructure.Commands;

internal static class Extensions
{
    internal static IServiceCollection AddCommands(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<ICommandDispatcher, CommandDispatcher>();
        services.Scan(s => s.FromAssemblies(assemblies) // With help of scrutor package register all command handlers
            .AddClasses(c => c
                .AssignableTo(typeof(ICommandHandler<>))
                .WithAttribute<DecoratorAttribute>())
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }

}