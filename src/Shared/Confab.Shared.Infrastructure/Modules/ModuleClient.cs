using Confab.Shared.Abstractions.Modules;

namespace Confab.Shared.Infrastructure.Modules;

internal class ModuleClient : IModuleClient
{
    private readonly IModuleRegistry _moduleRegistry;
    private readonly IModuleSerializer _moduleSerializer;
    
    public ModuleClient(IModuleRegistry moduleRegistry, IModuleSerializer moduleSerializer)
    {
        _moduleRegistry = moduleRegistry;
        _moduleSerializer = moduleSerializer;
    }
    
    public async Task PublishAsync(object message)
    {
        var key = message.GetType().Name;
        var registrations = _moduleRegistry.GetBroadcastRegistrations(key);

        var tasks = new List<Task>();
        
        foreach (var registration in registrations)
        {
            // We cannot do this like that, because there is type mismatch in runtime (types of message between modules are different)
            // registration.Action(message);

            var action = registration.Action;
            var receiverMessage = TranslateType(message, registration.ReceiverType);
            tasks.Add(action(receiverMessage));
        }

        await Task.WhenAll(tasks);
    }

    public Task SendAsync(string path, object request)
        => SendAsync<object>(path, request);

    public async Task<TResult> SendAsync<TResult>(string path, object request) where TResult : class
    {
        var registration = _moduleRegistry.GetRequestRegistration(path);
        if (registration is null)
        {
            throw new InvalidOperationException();
        }

        var receiverRequest = TranslateType(request, registration.RequestType);
        var result = await registration.Action(receiverRequest);

        return result is null ? null : TranslateType<TResult>(result);
    }

    private object TranslateType(object value, Type type)
        => _moduleSerializer.Deserialize(_moduleSerializer.Serialize(value), type);

    private T TranslateType<T>(object value)
        => _moduleSerializer.Deserialize<T>(_moduleSerializer.Serialize(value));
}