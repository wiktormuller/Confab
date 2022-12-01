using System.Runtime.CompilerServices;
using Confab.Shared.Abstractions;
using Confab.Shared.Infrastructure.Api;
using Confab.Shared.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Bootstrapper")]
namespace Confab.Shared.Infrastructure;

internal static class Extensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<IClock, UtcClock>();
        
        services.AddControllers()
            .ConfigureApplicationPartManager(manager =>
            {
                // Thanks to this setting the bootstrapper can see internal controllers from modules
                manager.FeatureProviders.Add(new InternalControllerFeatureProvider());
            });
            
        return services;
    }

    public static IApplicationBuilder UseInfrastructure(this IApplicationBuilder app)
    {
        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapGet("/", () => "Confab API!");
        });
        
        return app;
    }
}