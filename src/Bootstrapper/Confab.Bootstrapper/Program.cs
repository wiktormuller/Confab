using System.Reflection;
using Confab.Bootstrapper;
using Confab.Shared.Abstractions.Modules;
using Confab.Shared.Infrastructure;
using Confab.Shared.Infrastructure.Modules;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureModules();

IList<Assembly> _assemblies = ModuleLoader.LoadAssemblies(builder.Configuration);
IList<IModule> _modules = ModuleLoader.LoadModules(_assemblies);

builder.Services.AddInfrastructure(_assemblies, _modules);

foreach (var module in _modules)
{
    module.Register(builder.Services);
}

var app = builder.Build();

var logger = app.Services.GetService<ILogger<Program>>();

app.UseInfrastructure();

foreach (var module in _modules)
{
    module.Use(app);
}

logger.LogInformation($"Modules: {string.Join(", ", _modules.Select(x => x.Name))}");

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapGet("/", () => "Confab API!");
    endpoints.MapModuleInfo();
});

_assemblies.Clear();
_modules.Clear();

app.Run();