using Confab.Shared.Abstractions.Modules;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Confab.Modules.Attendances.Api;

internal class AttendancesModule : IModule
{
    public const string BasePath = "attendances-module";
    public string Name { get; } = "Attendances";
    public string Path => BasePath;

    public void Register(IServiceCollection services)
    {
        // @TODO: 
        //services.AddDomain()
        //    .AddApplication()
        //    .AddInfrastructure();
    }

    public void Use(IApplicationBuilder app)
    {
    }
}
