using System.Reflection;
using System.Runtime.CompilerServices;
using Confab.Shared.Abstractions;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure.Api;
using Confab.Shared.Infrastructure.Exceptions;
using Confab.Shared.Infrastructure.Services;
using Confab.Shared.Infrastructure.Time;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Bootstrapper")]
namespace Confab.Shared.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,
        IList<Assembly> assemblies, IList<IModule> modules)
    {
        var disabledModules = new List<string>();
        using (var serviceProvider = services.BuildServiceProvider())
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            foreach (var (key, value) in configuration.AsEnumerable())
            {
                if (!key.Contains(":module:enabled"))
                {
                    continue;
                }

                if (!bool.Parse(value))
                {
                    disabledModules.Add(key.Split(":")[0]);
                }
            }
        }
        
        services.AddErrorHandling();
        
        services.AddSingleton<IClock, UtcClock>();
        services.AddHostedService<AppInitializer>(); // Will ApplyMigrations for every known DbContext in solution automatically when application starts
        
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                // Thanks to this part there will not be any run-time classes from the disabled module
                var removedParts = new List<ApplicationPart>();
                foreach (var disabledModule in disabledModules)
                {
                    var parts = manager.ApplicationParts.Where(x => x.Name.Contains(disabledModule, 
                        StringComparison.InvariantCultureIgnoreCase));
                    removedParts.AddRange(parts);
                }

                foreach (var part in removedParts)
                {
                    manager.ApplicationParts.Remove(part);
                }
                
                // Thanks to this setting the bootstrapper can see internal controllers from modules
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
            
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseErrorHandling();
        app.UseRouting();

        return app;
    }

    public static T GetOptions<T>(this IServiceCollection services, string sectionName) where T : new()
    {
        // Building another serviceProvider is tricky
        using var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        return configuration.GetOptions<T>(sectionName);
    }

    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
    {
        var options = new T();
        configuration.GetSection(sectionName).Bind(options);
        return options;
    }
}