namespace Confab.Shared.Infrastructure.Modules;

public interface IModuleRegistry
{
    IEnumerable<ModuleBroadcastRegistration> GetBroadcastRegistrations(string key);
    void AddBroadcastAction(Type requestType, Func<object, Task> action);

    ModuleRequestRegistration GetRequestRegistration(string path);
    void AddRequestAction(string path, Type requestType, Type responseType, Func<object, Task<object>> action);
}