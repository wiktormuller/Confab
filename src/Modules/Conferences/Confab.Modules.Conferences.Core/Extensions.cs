using System.Runtime.CompilerServices;
using Confab.Modules.Conferences.Core.Policies;
using Confab.Modules.Conferences.Core.Repositories;
using Confab.Modules.Conferences.Core.Services;
using Microsoft.Extensions.DependencyInjection;

[assembly: InternalsVisibleTo("Confab.Modules.Conferences.Api")] // Now, it's visible to Api project ONLY // Now, it's visible to Api project ONLY
namespace Confab.Modules.Conferences.Core;

internal static class Extensions
{
    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services.AddSingleton<IHostRepository, InMemoryHostRepository>();
        services.AddSingleton<IConferenceRepository, InMemoryConferenceRepository>();
        
        services.AddSingleton<IConferenceDeletionPolicy, ConferenceDeletionPolicy>();
        services.AddSingleton<IHostDeletionPolicy, HostDeletionPolicy>();
        
        services.AddScoped<IHostService, HostService>();
        services.AddScoped<IConferenceService, ConferenceService>();
        
        return services; // Fluent
    }
}